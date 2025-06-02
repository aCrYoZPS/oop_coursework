using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Communities;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Communities;

namespace Wonderlust.UI.ViewModels;

public partial class CommunityFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly ICommunityService communityService;
    private readonly SessionManager sessionManager;

    public CommunityFormViewModel(ICommunityService communityService, SessionManager sessionManager)
    {
        this.communityService = communityService;
        this.sessionManager = sessionManager;
    }

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsValid))]
    private string name = string.Empty;

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsValid))]
    private string description = string.Empty;

    [ObservableProperty] private string action = "Create";

    private Community? community = new Community();

    public CommunityFormViewModel() { }

    public bool IsValid =>
        !string.IsNullOrWhiteSpace(Name) && !string.IsNullOrWhiteSpace(Description);

    [ObservableProperty] private bool isEditing = false;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("community", out var cobj) && cobj is Community comm)
        {
            community = comm;
            IsEditing = true;
            Name = comm.Name;
            Description = comm.Description;
            Action = "Edit";
        }
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    async Task SaveAsync()
    {
        if (community == null || sessionManager.CurrentUser == null)
        {
            return;
        }

        if (IsEditing)
        {
            community.Name = Name;
            community.Description = Description;
            var updated = await communityService.UpdateCommunityAsync(community);
            WeakReferenceMessenger.Default.Send(new CommunityEditedMessage(updated));
        }
        else
        {
            community = new Community(Name, Description, sessionManager.CurrentUser.Id);
            var created = await communityService.AddCommunityAsync(community);
            WeakReferenceMessenger.Default.Send(new CommunityAddedMessage(created));
        }

        await Shell.Current.GoToAsync(".."); // Navigate back
    }

    [RelayCommand]
    async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}