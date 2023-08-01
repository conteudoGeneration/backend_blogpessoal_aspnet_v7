using blogpessoal.Model;
using blogpessoal.Service;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace blogpessoal.Security.Implements
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<UserLogin?> Autenticar(UserLogin usuarioLogin)
        {
            string FotoDefault = "https://i.imgur.com/I8MfmC8.png";

            if (usuarioLogin == null || string.IsNullOrEmpty(usuarioLogin.Usuario) || string.IsNullOrEmpty(usuarioLogin.Senha))
                return null;

            var BuscaUsuario = await _userService.GetByUsuario(usuarioLogin.Usuario);

            if (BuscaUsuario is null)
                return null;

            if (!BCrypt.Net.BCrypt.Verify(usuarioLogin.Senha, BuscaUsuario.Senha))
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(Settings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuarioLogin.Usuario)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            usuarioLogin.Id = BuscaUsuario.Id;
            usuarioLogin.Nome = BuscaUsuario.Nome;
            usuarioLogin.Foto = BuscaUsuario.Foto is null ? FotoDefault : BuscaUsuario.Foto;
            usuarioLogin.Token = "Bearer " + tokenHandler.WriteToken(token).ToString();
            usuarioLogin.Senha = string.Empty;

            return usuarioLogin;

        }
    }
}

