using Category.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Category.Application.DTO
{
    public sealed record CategoryResponseDto(
     int Id,
     string Name,
     string? Description,
     Guid UserId,
     CategoryType Type);
}
