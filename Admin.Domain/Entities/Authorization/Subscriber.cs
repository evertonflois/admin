namespace Admin.Domain.Entities.Authorization;

public class Subscriber : BaseEntity
{
    public Guid? SubscriberId { get; set; }
    public string? Name { get; set; }    
    public string? Active { get; set; } 
}
