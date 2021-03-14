using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Ventas.Models;
using Ventas.Models.Common;
using Ventas.Models.Request;
using Ventas.Models.Response;
using Ventas.tools;

namespace Ventas.Services
{
    public class UserService : IUserService
    {
        private readonly AppSettings _appSettings;

        public UserService(IOptions<AppSettings> appSetings)
        {
            _appSettings = appSetings.Value;
        }
        public UserResponse Auth(AuthRequest model)
        {
            UserResponse userresponse = new UserResponse();
            using (var db = new VentasContext())
            {
                string spassword = Encrypt.GetSHA256(model.Password);
                var usuario = db.Usuarios.Where(d => d.Email == model.Email && d.Password == spassword).FirstOrDefault();
                if (usuario == null) return null;
                userresponse.Email = usuario.Email;
                userresponse.Token = GetToken(usuario);
            }
            return userresponse;
        }

        private string GetToken(Usuario usuario)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Email, usuario.Email)
                    }
                    ),
                Expires = DateTime.UtcNow.AddDays(60),
                SigningCredentials =  new SigningCredentials(
                    new SymmetricSecurityKey(llave),SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
