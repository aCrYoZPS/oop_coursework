using CommunityToolkit.Mvvm.Messaging.Messages;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Messages.Comments;

public class CommentEditedMessage(Comment comment) : ValueChangedMessage<Comment>(comment);