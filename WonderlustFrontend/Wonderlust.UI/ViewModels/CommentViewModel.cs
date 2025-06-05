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
    private Post post;

    public CommentViewModel(Comment comment, ICommentService commentService, SessionManager sessionManager, Post post)
    {
        this.commentService = commentService;
        this.sessionManager = sessionManager;
        this.post = post;
        Comment = comment;
        Replies = new ObservableCollection<CommentViewModel>(
            comment.Replies
                .Select(c => new CommentViewModel(c, commentService, sessionManager, post)));
    }

    public Comment Comment { get; }
    public string Content => Comment.Content;
    public DateTimeOffset LastUpdateDate => Comment.LastUpdateDate.ToLocalTime();

    public bool IsAuthor => sessionManager.CurrentUser?.Id == Comment.AuthorId;
    public bool IsValid => !Comment.Deleted;

    public ObservableCollection<CommentViewModel> Replies { get; }

    [RelayCommand(CanExecute = nameof(IsValid))]
    private async Task ReplyAsync(string replyText)
    {
        var navParams = new Dictionary<string, object>
        {
            ["postId"] = Comment.PostId, ["parentCommentId"] = Comment.Id, ["replyingToContent"] = Comment.Content,
            ["communityId"] = post.CommunityId,
        };

        await Shell.Current.GoToAsync(nameof(Pages.CommentFormPage), navParams);
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    private async Task EditCommentAsync()
    {
        var navParams = new Dictionary<string, object>
        {
            ["comment"] = Comment,
            ["postId"] = Comment.PostId,
            ["parentCommentId"] = Comment.Id,
            ["replyingToContent"] = Comment.Content,
            ["communityId"] = post.CommunityId,
        };

        await Shell.Current.GoToAsync(nameof(Pages.CommentFormPage), navParams);
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
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

        await commentService.DeleteCommentAsync(post.CommunityId, post.Id, Comment.Id);
        WeakReferenceMessenger.Default.Send(new CommentDeletedMessage(Comment.Id));
        Comment.Deleted = true;
    }
}