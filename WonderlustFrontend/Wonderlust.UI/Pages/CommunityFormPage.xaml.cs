using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Pages;

public partial class CommunityFormPage : ContentPage
{
    public CommunityFormPage(CommunityFormViewModel communityFormViewModel)
    {
        InitializeComponent();
        BindingContext = communityFormViewModel;
    }
}