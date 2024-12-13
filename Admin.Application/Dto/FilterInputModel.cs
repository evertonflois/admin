namespace Admin.Application.Dto;

public class FilterInputModel
{
    public string? Name { get; set; }
    public string? Value { get; set; }
    public string? MatchMode { get; set; }

    public FilterInputModel(string name, string value)
    {
        Name = name;
        Value = value;
    }
}
