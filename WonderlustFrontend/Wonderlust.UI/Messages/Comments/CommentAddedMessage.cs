using CommunityToolkit.Mvvm.Messaging.Messages;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Messages.Comments;

public class CommentAddedMessage(Comment comment) : ValueChangedMessage<Comment>(comment);