using Wonderlust.UI.Domain.Entities;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Wonderlust.UI.Messages.Posts;

public class PostDeletedMessage(Guid postId) : ValueChangedMessage<Guid>(postId);