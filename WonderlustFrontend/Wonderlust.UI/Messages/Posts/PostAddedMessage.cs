using Wonderlust.UI.Domain.Entities;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Wonderlust.UI.Messages.Posts;

public class PostAddedMessage(Post post) : ValueChangedMessage<Post>(post);