using Application.User.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.User.Ports
{
    public interface ILoginManager
    {
        Task<string> Authenticate(LoginDto loginDto);
    }
}
