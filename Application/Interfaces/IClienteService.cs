using Domain.Models;

namespace Application.Interfaces
{
    public interface IClienteService
    {
        Task<IEnumerable<Cliente>> GetAllClientsAsync();
        Task<Cliente?> GetClientByIdAsync(int id);
        Task<int> CreateClientAsync(Cliente client);
        Task<bool> UpdateClientAsync(Cliente client);
        Task<bool> DeleteClientAsync(int id);
    }
}
