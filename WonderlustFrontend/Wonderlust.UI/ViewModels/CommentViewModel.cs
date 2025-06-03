using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Comments;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Comments;

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

    public bool IsAuthor => sessionManager.CurrentUser?.Id == Comment.AuthorId;

    public ObservableCollection<CommentViewModel> Replies { get; }

    [RelayCommand]
    private async Task ReplyAsync(string replyText)
    {
        var navParams = new Dictionary<string, object>
        {
            ["postId"] = Comment.PostId, ["parentCommentId"] = Comment.Id, ["replyingToContent"] = Comment.Content
        };

        await Shell.Current.GoToAsync(nameof(Pages.CommentFormPage), navParams);
    }

    [RelayCommand]
    private async Task EditCommentAsync()
    {
        var navParams = new Dictionary<string, object>
        {
            ["comment"] = Comment,
            ["postId"] = Comment.PostId,
            ["parentCommentId"] = Comment.Id,
            ["replyingToContent"] = Comment.Content
        };

        await Shell.Current.GoToAsync(nameof(Pages.CommentFormPage), navParams);
    }

    [RelayCommand]
    private async Task DeleteCommentAsync()
    {
        if (Comment == null)
        {
            return;
        }

        var confirmed = await App.Current.MainPage.DisplayAlert(
            "Confirm", $"Delete comment?", "Yes", "Cancel");

        if (!confirmed)
        {
            return;
        }

        await commentService.DeleteCommentAsync(Comment.Id);
        WeakReferenceMessenger.Default.Send(new CommentDeletedMessage(Comment.Id));
    }
}