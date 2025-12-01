using Domain.Models;

namespace Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByDocumentoAsync(string documento);
        Task<int> UpsertAsync(Cliente cliente);
    }
}
