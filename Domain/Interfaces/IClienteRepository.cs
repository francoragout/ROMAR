namespace Domain.Interfaces
{
    public interface IClienteRepository
    {
        Task<IEnumerable<Models.Cliente>> GetAllClientsAsync();
        Task<Models.Cliente?> GetClientByIdAsync(int id);
        Task<int> CreateClientAsync(Models.Cliente client);
        Task<bool> UpdateClientAsync(Models.Cliente client);
        Task<bool> DeleteClientAsync(int id);
    }
}
