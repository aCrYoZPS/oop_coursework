using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Communities;
using Wonderlust.UI.Application.Services.Posts;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Communities;
using Wonderlust.UI.Messages.Posts;

namespace Wonderlust.UI.ViewModels;

public partial class PostsViewModel : ObservableObject, IQueryAttributable
{
    private readonly IPostService postService;
    private readonly ICommunityService communityService;
    private readonly SessionManager sessionManager;

    public PostsViewModel() { }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("community", out var cobj)
            && cobj is Community comm)
        {
            Community = comm;
            CommunityName = comm.Name;
            IsCreator = comm.CreatorId == sessionManager.CurrentUser.Id;
            _ = UpdatePosts();
        }
    }

    public PostsViewModel(IPostService postService, SessionManager sessionManager, ICommunityService communityService)
    {
        this.postService = postService;
        this.communityService = communityService;
        this.sessionManager = sessionManager;
        _ = UpdatePosts();

        WeakReferenceMessenger.Default.Register<PostAddedMessage>(
            this,
            (r, message) => { Posts.Insert(0, message.Value); });

        WeakReferenceMessenger.Default.Register<PostDeletedMessage>(this, (r, message) =>
        {
            var existing = Posts.FirstOrDefault(c => c.Id == message.Value);
            if (existing != null)
            {
                Posts.Remove(existing);
            }
        });

        WeakReferenceMessenger.Default.Register<PostEditedMessage>(
            this, (r, message) =>
            {
                var existing = Posts.FirstOrDefault(p => p.Id == message.Value.Id);
                if (existing != null)
                {
                    var index = Posts.IndexOf(existing);
                    Posts[index] = message.Value;
                }
            });
    }

    public ObservableCollection<Post> Posts { get; set; } = [];

    [ObservableProperty] private Community? community;
    [ObservableProperty] private string communityName;
    [ObservableProperty] private bool isCreator;

    [RelayCommand]
    private async Task UpdatePosts() => await GetPosts();

    private async Task GetPosts()
    {
        if (Community != null)
        {
            var posts = await postService.GetPosts(Community.Id);
            await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    Posts.Clear();
                    foreach (var post in posts)
                    {
                        Posts.Add(post);
                    }
                }
            );
        }
    }

    [RelayCommand]
    public async Task PostTappedAsync(Post? post)
    {
        if (post == null) return;

        var navParams = new Dictionary<string, object>
        {
            ["post"] = post
        };

        await Shell.Current.GoToAsync(nameof(Pages.Comments), navParams);
    }

    [RelayCommand]
    private async Task CreatePostAsync()
    {
        var navParams = new Dictionary<string, object> { ["communityId"] = Community.Id };
        await Shell.Current.GoToAsync(nameof(Pages.PostFormPage), navParams);
    }

    [RelayCommand]
    private async Task EditCommunityAsync()
    {
        if (Community == null)
        {
            return;
        }

        var navParams = new Dictionary<string, object> { ["community"] = Community };
        await Shell.Current.GoToAsync(nameof(Pages.CommunityFormPage), navParams);
    }

    [RelayCommand]
    async Task DeleteCommunityAsync()
    {
        if (Community == null)
        {
            return;
        }

        var confirmed = await App.Current.MainPage.DisplayAlert(
            "Confirm", $"Delete {Community.Name}?", "Yes", "Cancel");

        if (!confirmed)
        {
            return;
        }

        await communityService.DeleteCommunityAsync(Community.Id);
        WeakReferenceMessenger.Default.Send(new CommunityDeletedMessage(Community.Id));
        await Shell.Current.GoToAsync("..");
    }
}