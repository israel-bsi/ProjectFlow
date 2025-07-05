namespace ProjectFlow.Core.Models.DTOs.Filter;

public class FilterOption
{
    public string DisplayName { get; set; } = string.Empty;
    public string PropertyName { get; set; } = string.Empty;
    public Type Type { get; set; } = typeof(string);
    public override string ToString() => DisplayName;
}