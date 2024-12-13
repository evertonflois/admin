using Newtonsoft.Json;

namespace Admin.Application.Dto
{
    public class AuthViewModel
    {
        public string? Login { get; set; }
        public int CodigoEntidade { get; set; }
        public string? Token { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
        public IEnumerable<TransactionViewModel>? Transactions { get; set; }
        public IEnumerable<MenuViewModel>? Menu { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string? RefreshToken { get; set; }
    }
}
