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

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public async Task<Cliente?> GetByDocumentoAsync(string documento)
        {
            return await _repository.GetByDocumentoAsync(documento);
        }
        public async Task<int> UpsertAsync(Cliente cliente)
        {
            return await _repository.UpsertAsync(cliente);
        }
    }
}
