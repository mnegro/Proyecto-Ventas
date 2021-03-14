using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ventas.Models.Request;
using Ventas.Models.Response;

namespace Ventas.Services
{
   public interface IUserService
    {
        UserResponse Auth(AuthRequest model);
    }
}
