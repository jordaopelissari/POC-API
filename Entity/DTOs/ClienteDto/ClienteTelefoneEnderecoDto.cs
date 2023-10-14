using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ClienteDto;

public class ClienteTelefoneEnderecoDto : ClienteDto
{
    public string TipoTelefone { get; set; }
    public string DDD { get; set; }
    public string NumeroTelefone { get; set; }
    public string Rua { get; set; }
    public int Numero { get; set; }
    public string Complemento { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string CEP { get; set; }
}
