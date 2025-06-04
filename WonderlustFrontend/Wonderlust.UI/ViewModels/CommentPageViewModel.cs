using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Comments;
using Wonderlust.UI.Application.Services.Posts;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Comments;
using Wonderlust.UI.Messages.Posts;

namespace Wonderlust.UI.ViewModels;

public partial class CommentPageViewModel : ObservableObject, IQueryAttributable
{
    private readonly ICommentService commentService;
    private readonly IPostService postService;
    private readonly SessionManager sessionManager;

    [ObservableProperty] private Post? post;
    [ObservableProperty] private Comment? selectedComment;
    [ObservableProperty] private string? postTitle;
    [ObservableProperty] private bool isAuthor;
    [ObservableProperty] private string? content;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsCommentButtonEnabled))]
    private string? commentContent;

    public bool IsCommentButtonEnabled => !string.IsNullOrWhiteSpace(CommentContent);

    public ObservableCollection<CommentViewModel> Comments { get; } = [];

    public CommentPageViewModel() { }

    public CommentPageViewModel(ICommentService commentService, IPostService postService, SessionManager sessionManager)
    {
        this.commentService = commentService;
        this.postService = postService;
        this.sessionManager = sessionManager;
        _ = UpdateComments();
        WeakReferenceMessenger.Default.Register<CommentAddedMessage>(this, (r, message) => _ = UpdateComments());
        WeakReferenceMessenger.Default.Register<CommentDeletedMessage>(this, (r, message) => _ = UpdateComments());
        WeakReferenceMessenger.Default.Register<CommentEditedMessage>(this, (r, message) => _ = UpdateComments());
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("post", out var pobj)
            && pobj is Post postAttr)
        {
            Post = postAttr;
            PostTitle = postAttr.Title;
            Content = postAttr.Content;
            IsAuthor = postAttr.AuthorId == sessionManager.CurrentUser?.Id;
            _ = UpdateComments();
        }
    }

    [RelayCommand]
    private async Task UpdateComments() => await GetComments();

    private async Task GetComments()
    {
        if (Post != null)
        {
            var comments = await commentService.GetComments(Post.CommunityId, Post.Id);
            await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Comments.Clear();
                    foreach (var comment in comments)
                    {
                        Comments.Add(new CommentViewModel(comment, commentService, sessionManager, Post));
                    }
                }
            );
        }
    }

    [RelayCommand]
    private async Task EditPostAsync()
    {
        if (Post == null)
        {
            return;
        }

        var navParams = new Dictionary<string, object> { ["post"] = Post, ["communityId"] = Post.CommunityId };
        await Shell.Current.GoToAsync(nameof(Pages.PostFormPage), navParams);
    }

    [RelayCommand]
    private async Task DeletePostAsync()
    {
        if (Post == null)
        {
            return;
        }

        var confirmed = await App.Current.MainPage.DisplayAlert(
            "Confirm", $"Delete {Post.Title}?", "Yes", "Cancel");

        if (!confirmed)
        {
            return;
        }

        await postService.DeletePostAsync(Post.Id, Post.CommunityId);
        WeakReferenceMessenger.Default.Send(new PostDeletedMessage(Post.Id));
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CreateCommentAsync()
    {
        if (Post == null)
        {
            return;
        }

        var navParams = new Dictionary<string, object>
        {
            ["postId"] = Post.Id, ["replyingToContent"] = Post.Content ?? "",
            ["communityId"] = Post.CommunityId
        };

        await Shell.Current.GoToAsync(nameof(Pages.CommentFormPage), navParams);
    }

    void HandleOption(string option)
    {
        Console.WriteLine($"Selected: {option}");
    }
}