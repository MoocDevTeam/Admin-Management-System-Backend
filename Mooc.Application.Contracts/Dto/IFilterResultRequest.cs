using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mooc.Application.Contracts.Dto;

public interface IFilterResultRequest
{
    string? Filter { get; set; }
}
