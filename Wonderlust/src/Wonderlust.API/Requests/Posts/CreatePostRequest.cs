using System.ComponentModel.DataAnnotations;

namespace Wonderlust.API.Requests.Posts;

public record CreatePostRequest(
    [Required] string Title,
    string Content,
    Guid? ImageId
)
{
    public Guid AuthorId { get; set; }
    public Guid CommunityId { get; set; }
};