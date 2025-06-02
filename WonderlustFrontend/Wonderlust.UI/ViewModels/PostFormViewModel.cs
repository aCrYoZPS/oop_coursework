using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Posts;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Posts;

namespace Wonderlust.UI.ViewModels;

public partial class PostFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly IPostService postService;
    private readonly SessionManager sessionManager;
    private Guid? communityId;

    public PostFormViewModel(IPostService postService, SessionManager sessionManager)
    {
        this.postService = postService;
        this.sessionManager = sessionManager;
    }

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsValid))]
    private string title = string.Empty;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsValid))]
    private string content = string.Empty;

    [ObservableProperty] private string action = "Create";

    private Post? post = new Post();

    public PostFormViewModel() { }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Title) && !string.IsNullOrWhiteSpace(Content);

    [ObservableProperty] private bool isEditing = false;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("post", out var cobj) && cobj is Post pst)
        {
            post = pst;
            IsEditing = true;
            Title = pst.Title;
            Content = pst.Content ?? "";
            Action = "Edit";
        }

        if (query.TryGetValue("communityId", out var value) && value is Guid id)
        {
            communityId = id;
        }
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    async Task SaveAsync()
    {
        if (post == null || sessionManager.CurrentUser == null || communityId == null)
        {
            return;
        }

        if (IsEditing)
        {
            post.Title = Title;
            post.Content = Content;
            var updated = await postService.UpdatePostAsync(post);
            WeakReferenceMessenger.Default.Send(new PostEditedMessage(updated));
        }
        else
        {
            post = new Post(Title, Content, communityId.Value, sessionManager.CurrentUser.Id);
            var created = await postService.AddPostAsync(post);
            WeakReferenceMessenger.Default.Send(new PostAddedMessage(created));
        }

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}