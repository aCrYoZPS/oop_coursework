using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Wonderlust.UI.Application.Services.Comments;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.ViewModels;

public partial class CommentViewModel : ObservableObject
{
    private readonly ICommentService commentService;
    private readonly SessionManager sessionManager;

    public CommentViewModel(Comment comment, ICommentService commentService, SessionManager sessionManager)
    {
        this.commentService = commentService;
        this.sessionManager = sessionManager;
        Comment = comment;
        Replies = new ObservableCollection<CommentViewModel>(
            comment.Replies
                .Select(c => new CommentViewModel(c, commentService, sessionManager)));
    }

    public Comment Comment { get; }

    public string Content => Comment.Content;
    public DateTimeOffset CreationDate => Comment.CreationDate;

    public ObservableCollection<CommentViewModel> Replies { get; }

    [RelayCommand]
    public async Task ReplyAsync(string replyText)
    {
        var reply = new Comment(Content, sessionManager.CurrentUser.Id, Comment.PostId, Comment.Id);
        var newComment = await commentService.AddCommentAsync(reply);
        Replies.Add(new CommentViewModel(newComment, commentService, sessionManager));
    }
}