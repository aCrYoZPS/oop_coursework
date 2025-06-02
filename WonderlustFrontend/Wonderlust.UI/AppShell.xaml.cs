namespace Wonderlust.UI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(Pages.Posts), typeof(Pages.Posts));
        Routing.RegisterRoute(nameof(Pages.Comments), typeof(Pages.Comments));
        Routing.RegisterRoute(nameof(Pages.CommunityFormPage), typeof(Pages.CommunityFormPage));
        Routing.RegisterRoute(nameof(Pages.PostFormPage), typeof(Pages.PostFormPage));
    }
}