using System.Net.Http.Headers;
using System.Text;
using SerializerLib.Json;
using Wonderlust.UI.Application.Services.Comments.Requests;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Application.Services.Comments;

public class CommentService(HttpClient httpClient, SessionManager.SessionManager sessionManager) : ICommentService
{
    private static List<Comment> comments =
    [
        new Comment(Guid.Parse("a3d14cea-5f60-4ddf-9695-72f987759a82"), "cntnt", Guid.NewGuid(), Guid.NewGuid(), null)
        {
            Replies =
            [
                new Comment(Guid.Parse("f7809e34-a118-405c-9e13-c4d6006b5a86"), "Reply1", Guid.NewGuid(),
                    Guid.NewGuid(), null)
                {
                    Replies =
                    [
                        new Comment(Guid.Parse("016a8722-3803-4215-ac77-7df39479259d"), "InnerReply", Guid.NewGuid(),
                            Guid.NewGuid(), null),
                    ]
                },
                new Comment(Guid.Parse("4f0f0f7c-51f0-4bc0-a138-dcca3d7267cc"), "Reply2", Guid.NewGuid(),
                    Guid.NewGuid(), null),
            ]
        },
    ];

    public async Task<IEnumerable<Comment>> GetComments(Guid communityId, Guid postId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = httpClient.GetAsync($"{communityId}/{postId}/comments").GetAwaiter().GetResult();
        comments =
            JsonSerializer.Deserialize<List<Comment>>(response.Content.ReadAsStringAsync().GetAwaiter().GetResult()) ??
            [];

        foreach (var comment in comments.ToArray())
        {
            if (comment.ParentCommentId != null)
            {
                comments.First(c => c.Id == comment.ParentCommentId).Replies.Add(comment);
                comments.Remove(comment);
            }
        }

        return comments ?? [];
    }

    private void FlattenList(List<Comment> list, List<Comment> flattened)
    {
        foreach (var comment in list)
        {
            flattened.Add(comment);
            if (comment.Replies.Count != 0)
            {
                FlattenList(comment.Replies, flattened);
            }
        }
    }

    public async Task<Comment> AddCommentAsync(Guid communityId, Comment comment)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent
        (
            System.Text.Json.JsonSerializer.Serialize(new AddCommentRequest(comment.Content, comment.ParentCommentId)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PostAsync($"{communityId}/{comment.PostId}/comments", content);
        response.EnsureSuccessStatusCode();

        if (comment.ParentCommentId == null)
        {
            comments.Insert(0, comment);
        }
        else
        {
            var flattenedComments = new List<Comment>();
            FlattenList(comments, flattenedComments);

            var parentComment = flattenedComments.First(c => c.Id == comment.ParentCommentId);
            parentComment.Replies.Insert(0, comment);
        }

        return comment;
    }


    public async Task<Comment> UpdateCommentAsync(Guid communityId, Comment comment)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var content = new StringContent
        (
            JsonSerializer.Serialize(new UpdateCommentRequest(comment.Content)),
            Encoding.UTF8,
            "application/json"
        );

        var response = await httpClient.PutAsync($"{communityId}/{comment.PostId}/comments/{comment.Id}", content);
        response.EnsureSuccessStatusCode();

        var flattenedComments = new List<Comment>();
        FlattenList(comments, flattenedComments);
        var index = -1;
        for (var i = 0; i < flattenedComments.Count; ++i)
        {
            if (flattenedComments[i].Id == comment.Id)
            {
                index = i;
            }
        }

        if (index != -1)
        {
            comments[index] = comment;
        }

        return comment;
    }

    public async Task DeleteCommentAsync(Guid communityId, Guid postId, Guid commentId)
    {
        var token = await sessionManager.GetToken();
        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await httpClient.DeleteAsync($"{communityId}/{postId}/comments/{commentId}");
        response.EnsureSuccessStatusCode();

        var flattenedComments = new List<Comment>();
        FlattenList(comments, flattenedComments);

        var existingComment = flattenedComments.FirstOrDefault(c => c.Id == commentId);
        if (existingComment != null)
        {
            existingComment.Content = "Deleted";
        }
    }
}