using Entity.DTOs.ClienteDto;
using Repository.Interfaces.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces;

public interface IClienteRepository : IRepository<ClienteDto>
{
    IEnumerable<ClienteDto> SelecionarTodosClientes();
    ClienteTelefoneEnderecoDto ObterDadosClienteTelefoneEndereco(int codigo);
    IEnumerable<ClienteTelefoneEnderecoDto> ObterTodosDadosClienteTelefoneEndereco();
    ClienteDto SelecionarClientePorCodigo(int codigo);
    void AtualizarNomeCliente(ClienteDto cliente);
}
