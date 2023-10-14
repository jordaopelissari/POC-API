using Entity.DTOs.ClienteDto;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiPoc : ControllerBase
    {
        private readonly ILogger<ApiPoc> _logger;
        private readonly IClienteRepository _repositoryCliente;

        public ApiPoc(ILogger<ApiPoc> logger, IClienteRepository repositoryCliente)
        {
            _logger = logger;
            _repositoryCliente = repositoryCliente;
        }

        [HttpGet(Name = "Get")]
        public string Get()
        {
            var a = _repositoryCliente.SelecionarTodosClientes();
            var b = _repositoryCliente.ObterDadosClienteTelefoneEndereco(1);
            var c = _repositoryCliente.SelecionarClientePorCodigo(1);
            var cliente = new ClienteDto { Codigo = 1, Nome = "João" };

            _repositoryCliente.AtualizarNomeCliente(cliente);
            var e = _repositoryCliente.SelecionarTodosClientes();

            return "";
        }
    }
}