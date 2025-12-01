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

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            var query = "SELECT * FROM Clientes ORDER BY IdCliente";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<Cliente>(query);
        }

        public async Task<Cliente?> GetByDocumentoAsync(string documento)
        {

            var query = "SELECT * FROM Clientes WHERE Documento = @Documento";

            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Cliente>(query, new { Documento = documento });
        }

        public async Task<int> CreateAsync(Cliente cliente)
        {
            var query = @"INSERT INTO Clientes (Nombre, Apellido, Documento, TipoDocumento, Email, Telefono, Direccion, Localidad, Provincia, CodigoPostal, FechaAlta)
                        VALUES (@Nombre, @Apellido, @Documento, @TipoDocumento, @Email, @Telefono, @Direccion, @Localidad, @Provincia, @CodigoPostal, @FechaAlta);
                        SELECT CAST(SCOPE_IDENTITY() as int);";

            using var connection = _context.CreateConnection();
            var id = await connection.QuerySingleAsync<int>(query, cliente);
            return id;
        }
        public async Task<int> UpsertAsync(Cliente cliente)
        {
            var existing = await GetByDocumentoAsync(cliente.Documento);
            if (existing == null)
            {
                return await CreateAsync(cliente);
            }
            else
            {
                cliente.IdCliente = existing.IdCliente;
                await UpdateAsync(cliente);
                return cliente.IdCliente;
            }
        }

        public async Task<bool> UpdateAsync(Cliente cliente)
        {
            var query = @"UPDATE Clientes SET
                            Nombre = @Nombre,
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
            var rows = await connection.ExecuteAsync(query, cliente);
            return rows > 0;
        }
    }
}
