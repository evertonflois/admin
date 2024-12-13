namespace Admin.Domain.Entities;

public class Filter
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? MatchMode { get; set; }

    public Filter(string? name, string? value)
    {
        Name = name;
        Value = value;
        MatchMode = "equal";
    }
}
