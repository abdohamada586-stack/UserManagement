using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace UserManagement.Service.Interfaces
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(
            string userId,
            string userName,
            string email);
    }
}
