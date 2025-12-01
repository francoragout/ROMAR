using Application.Interfaces;
using Application.Services;
using Domain.Models;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    // Simple ETL: authenticate, fetch external clients, map and upsert into local DB
    public class ExternalEtlService
    {
        private readonly HttpClient _httpClient;
        private readonly IClienteService _clienteService;

        public ExternalEtlService(HttpClient httpClient, IClienteService clienteService)
        {
            _httpClient = httpClient;
            _clienteService = clienteService;
        }

        public async Task<IEnumerable<Cliente>> RunEtlAsync(CancellationToken cancellationToken = default)
        {
            // 1. Authenticate
            var loginUrl = "http://gemsa.ddns.net:9132/api/Auth/login";
            var loginBody = new { userName = "Prueba", password = "PruebaRomar" };
            var loginContent = new StringContent(JsonSerializer.Serialize(loginBody), Encoding.UTF8, "application/json");

            using var loginResp = await _httpClient.PostAsync(loginUrl, loginContent, cancellationToken);
            if (!loginResp.IsSuccessStatusCode)
                return Enumerable.Empty<Cliente>();

            using var loginStream = await loginResp.Content.ReadAsStreamAsync(cancellationToken);
            using var loginDoc = await JsonDocument.ParseAsync(loginStream, cancellationToken: cancellationToken);
            if (!loginDoc.RootElement.TryGetProperty("token", out var tokenEl))
                return Enumerable.Empty<Cliente>();

            var token = tokenEl.GetString();
            if (string.IsNullOrEmpty(token))
                return Enumerable.Empty<Cliente>();

            // 2. Get clientes
            var clientesUrl = "http://gemsa.ddns.net:9132/api/ClientesFacturas/clientes";
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var clientesResp = await _httpClient.GetAsync(clientesUrl, cancellationToken);
            if (!clientesResp.IsSuccessStatusCode)
                return Enumerable.Empty<Cliente>();

            var clientesJson = await clientesResp.Content.ReadAsStringAsync(cancellationToken);
            var externalClientes = JsonSerializer.Deserialize<List<ExternalCliente>>(clientesJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            if (externalClientes == null)
                return Enumerable.Empty<Cliente>();

            var result = new List<Cliente>();
            foreach (var ext in externalClientes)
            {
                var mapped = Map(ext);
                await _clienteService.UpsertAsync(mapped);
                result.Add(mapped);
            }

            // 3. Return local DB clients (fresh)
            return await _clienteService.GetAllAsync();
        }

        private Cliente Map(ExternalCliente ext)
        {
            return new Cliente
            {
                // Assume ext.idcliente maps to nothing in local Id; Upsert uses documento
                Nombre = ext.nombre ?? string.Empty,
                Apellido = ext.apellido ?? string.Empty,
                Documento = ext.documento ?? string.Empty,
                TipoDocumento = ext.tipo_documento ?? string.Empty,
                Email = ext.email ?? string.Empty,
                Telefono = ext.telefono ?? string.Empty,
                Direccion = ext.direccion ?? string.Empty,
                Localidad = ext.localidad ?? string.Empty,
                Provincia = ext.provincia ?? string.Empty,
                CodigoPostal = ext.codigo_postal ?? string.Empty,
                FechaAlta = ext.fecha_alta ?? DateTime.MinValue
            };
        }

        private class ExternalCliente
        {
            public int idcliente { get; set; }
            public string? nombre { get; set; }
            public string? apellido { get; set; }
            public string? documento { get; set; }
            public string? tipo_documento { get; set; }
            public string? email { get; set; }
            public string? telefono { get; set; }
            public string? direccion { get; set; }
            public string? localidad { get; set; }
            public string? provincia { get; set; }
            public string? codigo_postal { get; set; }
            public DateTime? fecha_alta { get; set; }
        }
    }
}