using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Pages;

public partial class CommentFormPage : ContentPage
{
    public CommentFormPage(CommentFormViewModel commentFormViewModel)
    {
        InitializeComponent();
        BindingContext = commentFormViewModel;
    }
}