using Application.Interfaces;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;

        public ClienteService(IClienteRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Cliente>> GetAllClientsAsync()
        {
            return await _repository.GetAllClientsAsync();
        }

        public async Task<Cliente?> GetClientByIdAsync(int id)
        {
            return await _repository.GetClientByIdAsync(id);
        }

        public async Task<int> CreateClientAsync(Cliente client)
        {
            return await _repository.CreateClientAsync(client);
        }

        public async Task<bool> UpdateClientAsync(Cliente client)
        {
            return await _repository.UpdateClientAsync(client);
        }

        public async Task<bool> DeleteClientAsync(int id)
        {
            return await _repository.DeleteClientAsync(id);
        }
    }
}
