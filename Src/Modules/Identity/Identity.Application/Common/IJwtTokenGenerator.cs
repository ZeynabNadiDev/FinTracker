using Identity.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Application.Common
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
