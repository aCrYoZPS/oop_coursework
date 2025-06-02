using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Pages;

public partial class PostFormPage : ContentPage
{
    public PostFormPage(PostFormViewModel postFormViewModel)
    {
        InitializeComponent();
        BindingContext = postFormViewModel;
    }
}