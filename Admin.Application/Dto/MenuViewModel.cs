using System.Text.Json.Serialization;

namespace Admin.Application.Dto
{
    public class MenuViewModel
    {
        public string Label { get; set; }
        public string? Icon { get; set; }
        public string[]? RouterLink { get; set; }        
        public MenuViewModel[]? Items { get; set; }
        [JsonIgnore]
        public string? CdAgrpMenu { get; set; }
    }
}
