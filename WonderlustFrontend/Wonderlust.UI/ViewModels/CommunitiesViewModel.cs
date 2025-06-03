using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Communities;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Communities;

namespace Wonderlust.UI.ViewModels;

public partial class CommunitiesViewModel : ObservableObject
{
    private readonly ICommunityService communityService;

    public CommunitiesViewModel() { }

    public CommunitiesViewModel(ICommunityService communityService)
    {
        this.communityService = communityService;
        _ = UpdateCommunities();
        WeakReferenceMessenger.Default.Register<CommunityAddedMessage>(
            this,
            (r, message) => { Communities.Insert(0, message.Value); });

        WeakReferenceMessenger.Default.Register<CommunityDeletedMessage>(this, (r, message) =>
        {
            var existing = Communities.FirstOrDefault(c => c.Id == message.Value);
            if (existing != null)
            {
                Communities.Remove(existing);
            }
        });

        WeakReferenceMessenger.Default.Register<CommunityEditedMessage>(
            this, (r, message) =>
            {
                var existing = Communities.FirstOrDefault(c => c.Id == message.Value.Id);
                if (existing != null)
                {
                    var index = Communities.IndexOf(existing);
                    Communities[index] = message.Value;
                }
            });

        WeakReferenceMessenger.Default.Register<SubscriptionMessage>(this,
            (r, message) => { _ = UpdateCommunities(); });
    }

    public ObservableCollection<Community> Communities { get; set; } = [];

    [ObservableProperty] private Community? selectedCommunity;

    [RelayCommand]
    private async Task CreateCommunityAsync()
    {
        await Shell.Current.GoToAsync(nameof(Pages.CommunityFormPage));
    }

    partial void OnSelectedCommunityChanged(Community? value)
    {
        if (value == null)
        {
            return;
        }

        OpenCommunityCommand.Execute(value);

        SelectedCommunity = null;
    }

    [RelayCommand]
    private async Task OpenCommunityAsync(Community? community)
    {
        if (community == null) return;

        var navParams = new Dictionary<string, object>
        {
            ["community"] = community
        };

        await Shell.Current.GoToAsync(nameof(Pages.Posts), navParams);
    }

    [RelayCommand]
    private async Task UpdateCommunities() => await GetCommunities();

    private async Task GetCommunities()
    {
        var communities = await communityService.GetCommunities();
        await MainThread.InvokeOnMainThreadAsync(() =>
            {
                Communities.Clear();
                foreach (var community in communities)
                {
                    Communities.Add(community);
                }
            }
        );
    }
}