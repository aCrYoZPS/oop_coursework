using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Pages;

public partial class Comments : ContentPage
{
    public Comments(CommentPageViewModel commentPageViewModel)
    {
        InitializeComponent();
        BindingContext = commentPageViewModel;
    }
}