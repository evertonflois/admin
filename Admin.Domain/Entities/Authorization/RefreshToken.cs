namespace Admin.Domain.Entities.Authorization;

using Admin.Domain.Attributes;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RefreshToken
{
    [Key]
    [JsonIgnore]
    [EntityField(IsKey = true)]
    public Guid? SubscriberId { get; set; }
    [EntityField(IsKey = true)]
    public string? Login { get; set; }
    [EntityField(IsKey = true)]
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public string ReasonRevoked { get; set; }
    [EntityField(Persists = false)]
    public bool IsExpired => DateTime.UtcNow >= Expires;
    [EntityField(Persists = false)]
    public bool IsRevoked => Revoked != null;
    [EntityField(Persists = false)]
    public bool IsActive => !IsRevoked && !IsExpired;
}