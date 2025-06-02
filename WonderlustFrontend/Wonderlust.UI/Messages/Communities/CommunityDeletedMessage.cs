using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Wonderlust.UI.Messages.Communities;

public class CommunityDeletedMessage(Guid communityId) : ValueChangedMessage<Guid>(communityId);
