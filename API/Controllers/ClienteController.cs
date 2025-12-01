using Application.Interfaces;
using Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _service;
        private readonly ExternalEtlService _etl;

        public ClientesController(IClienteService service, ExternalEtlService etl)
        {
            _service = service;
            _etl = etl;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            await _etl.RunEtlAsync();
            return Ok(await _service.GetAllAsync());
        }
    }
}
