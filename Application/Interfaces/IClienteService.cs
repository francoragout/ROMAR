using Domain.Models;

namespace Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllAsync();
        Task<Cliente?> GetByDocumentoAsync(string documento);
        Task<int> UpsertAsync(Cliente cliente);
    }
}
