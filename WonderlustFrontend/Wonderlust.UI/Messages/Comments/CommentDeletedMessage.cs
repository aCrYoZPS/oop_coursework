using CommunityToolkit.Mvvm.Messaging.Messages;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Messages.Comments;

public class CommentDeletedMessage(Guid commentId) : ValueChangedMessage<Guid>(commentId);