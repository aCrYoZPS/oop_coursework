namespace Wonderlust.UI.Application.Services.Communities.Requests;

public class AddCommunityRequest(string name, string description)
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
}