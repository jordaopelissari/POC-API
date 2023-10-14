using Entity.DTOs.ClienteDto;
using Microsoft.Extensions.Logging;
using Repository.Context;
using Repository.Interfaces;
using Repository.Interfaces.Base;
using Repository.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories;

public class ClienteRepository : Repository<ClienteDto>, IClienteRepository
{
    public ClienteRepository(IUnitOfWork unitOfWork,
    DbContext context,
    ILogger<Repository<ClienteDto>> logger) : base(unitOfWork, context, logger)
    {
    }

    public IEnumerable<ClienteDto> SelecionarTodosClientes()
    {
        return this.CommandQuery<ClienteDto>("SelecionarTodosClientes");
    }

    public ClienteTelefoneEnderecoDto ObterDadosClienteTelefoneEndereco(int codigo)
    {
        return this.CommandQuerySingleOrDefault<ClienteTelefoneEnderecoDto>("ObterDadosClienteTelefoneEndereco", new Dictionary<string, object> { { "@CodigoCliente", codigo } });
    }

    public IEnumerable<ClienteTelefoneEnderecoDto> ObterTodosDadosClienteTelefoneEndereco()
    {
        return this.CommandQuery<ClienteTelefoneEnderecoDto>("ObterTodosDadosClienteTelefoneEndereco");
    }

    public ClienteDto SelecionarClientePorCodigo(int codigo)
    {
        return this.CommandQuerySingleOrDefault<ClienteDto>("SelecionarClientePorCodigo", new Dictionary<string, object> { { "@CodigoCliente", codigo } });
    }

    public void AtualizarNomeCliente(ClienteDto cliente)
    {
        var dict = new Dictionary<string, object>() 
        {
            {"@Codigo", cliente.Codigo },
            {"@NovoNome", cliente.Nome }
        };

        this.CommandExecute("AtualizarNomeCliente", dict);
    }

}
