using Dapper;
using Domain.Interfaces;
using Domain.Models;
using Infraestructure.Database;

namespace Infraestructure.Repositories
{
    public class ClientRepository : IClienteRepository
    {
        private readonly DapperContext _context;

        public ClientRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientsAsync()
        {
            var query = "SELECT * FROM Clientes";
            using var connection = _context.CreateConnection();
            var clients = await connection.QueryAsync<Cliente>(query);
            return clients;
        }

        public async Task<Cliente?> GetClientByIdAsync(int id)
        {
            var query = "SELECT * FROM Clientes WHERE IdCliente = @Id";
            using var connection = _context.CreateConnection();
            var client = await connection.QuerySingleOrDefaultAsync<Cliente>(query, new { Id = id });
            return client;
        }

        public async Task<int> CreateClientAsync(Cliente client)
        {
            var query = @"
                INSERT INTO Clientes (Nombre, Apellido, Documento, TipoDocumento, Email, Telefono, Direccion, Localidad, Provincia, CodigoPostal, FechaAlta)
                VALUES (@Nombre, @Apellido, @Documento, @TipoDocumento, @Email, @Telefono, @Direccion, @Localidad, @Provincia, @CodigoPostal, @FechaAlta);
                SELECT CAST(SCOPE_IDENTITY() as int)";
            using var connection = _context.CreateConnection();
            var id = await connection.QuerySingleAsync<int>(query, client);
            return id;
        }

        public async Task<bool> UpdateClientAsync(Cliente client)
        {
            var query = @"
                UPDATE Clientes
                SET Nombre = @Nombre,
                    Apellido = @Apellido,
                    Documento = @Documento,
                    TipoDocumento = @TipoDocumento,
                    Email = @Email,
                    Telefono = @Telefono,
                    Direccion = @Direccion,
                    Localidad = @Localidad,
                    Provincia = @Provincia,
                    CodigoPostal = @CodigoPostal,
                    FechaAlta = @FechaAlta
                WHERE IdCliente = @IdCliente";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, client);
            return affectedRows > 0;
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            var query = "DELETE FROM Clientes WHERE IdCliente = @Id";
            using var connection = _context.CreateConnection();
            var affectedRows = await connection.ExecuteAsync(query, new { Id = id });
            return affectedRows > 0;
        }
    }
}
