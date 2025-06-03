using System.Globalization;
using Wonderlust.UI.Application.Services.Subscriptions;
using Wonderlust.UI.Application.SessionManager;
using Wonderlust.UI.Domain.Entities;

namespace Wonderlust.UI.Converters;

public class SubscribedToBrushConverter : IValueConverter
{
    private ISubscriptionService subscriptionService;
    private ISubscriptionService SubscriptionService => subscriptionService ??= GetService<ISubscriptionService>();

    private SessionManager sessionManager;
    private SessionManager SessionManager => sessionManager ??= GetService<SessionManager>();

    public SubscribedToBrushConverter() { }

    private T GetService<T>()
    {
        var service = MauiProgram.Services.GetService<T>();

        if (service == null)
        {
            return default(T);
        }

        return service;
    }

    public object Convert(object value, Type targetType, object parameter, CultureInfo ci)
    {
        if (value is Community c)
        {
            if (SubscriptionService.IsSubscribed(c.Id, SessionManager.CurrentUser.Id).GetAwaiter().GetResult())
            {
                return new SolidColorBrush(Color.FromArgb("#9880e5"));
            }
        }

        return new SolidColorBrush(Color.FromArgb("#404040"));
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo ci) =>
        throw new NotImplementedException();
}