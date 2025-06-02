using Wonderlust.UI.Domain.Entities;
using CommunityToolkit.Mvvm.Messaging.Messages;

namespace Wonderlust.UI.Messages.Communities;

public class CommunityEditedMessage(Community community) : ValueChangedMessage<Community>(community);