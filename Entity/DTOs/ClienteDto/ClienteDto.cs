using Entity.DTOs.BaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.DTOs.ClienteDto;

public class ClienteDto : BaseDto
{
    public string Nome { get; set; }
    public string CPF { get; set; }
}
