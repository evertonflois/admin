using System.Text.Json.Serialization;

namespace PSFat.Application.Areas.Autorizacao.E_710_User.Commands.AutenticarUsuario
{
    public class UsuarioAutenticadoDto
    {
        public string? login { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public UsuarioAutenticadoDto(AutenticarUsuarioCommand user, string jwtToken, string refreshToken)
        {
            login = user.Login;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}
