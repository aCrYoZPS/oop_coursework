using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Wonderlust.UI.Application.Services.Comments;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;
using Wonderlust.UI.Messages.Comments;

namespace Wonderlust.UI.ViewModels;

public partial class CommentFormViewModel : ObservableObject, IQueryAttributable
{
    private readonly ICommentService commentService;
    private readonly SessionManager sessionManager;
    private Guid? postId;
    private Guid? parentCommentId;

    public CommentFormViewModel(ICommentService commentService, SessionManager sessionManager)
    {
        this.commentService = commentService;
        this.sessionManager = sessionManager;
    }

    [ObservableProperty] [NotifyPropertyChangedFor(nameof(IsValid))]
    private string content = string.Empty;

    [ObservableProperty] private string replyingToContent = string.Empty;

    [ObservableProperty] private string action = "Create";

    private Comment? comment;

    public CommentFormViewModel() { }

    public bool IsValid => !string.IsNullOrWhiteSpace(Content);

    [ObservableProperty] private bool isEditing = false;

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("comment", out var cobj) && cobj is Comment comm)
        {
            comment = comm;
            IsEditing = true;
            Content = comm.Content;
            Action = "Edit";
        }

        if (query.TryGetValue("postId", out var postIdValue) && postIdValue is Guid postIdAttr)
        {
            postId = postIdAttr;
        }

        if (query.TryGetValue("parentCommentId", out var parentCommentIdValue) &&
            parentCommentIdValue is Guid parentCommentIdAttr)
        {
            parentCommentId = parentCommentIdAttr;
        }

        if (query.TryGetValue("replyingToContent", out var replToContent) &&
            replToContent is string replyingToContentAttr)
        {
            ReplyingToContent = replyingToContentAttr;
        }
    }

    [RelayCommand(CanExecute = nameof(IsValid))]
    async Task SaveAsync()
    {
        if (sessionManager.CurrentUser == null)
        {
            return;
        }

        if (IsEditing)
        {
            if (comment == null)
            {
                return;
            }

            comment.Content = Content;
            var updated = await commentService.UpdateCommentAsync(comment);
            WeakReferenceMessenger.Default.Send(new CommentEditedMessage(updated));
        }
        else
        {
            comment = new Comment(Guid.NewGuid(), Content, sessionManager.CurrentUser.Id, postId.Value,
                parentCommentId);
            var created = await commentService.AddCommentAsync(comment);
            WeakReferenceMessenger.Default.Send(new CommentAddedMessage(created));
        }

        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    async Task CancelAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}