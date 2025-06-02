using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Pages;

public partial class Posts : ContentPage
{
    public Posts(PostsViewModel postsViewModel)
    {
        InitializeComponent();
        BindingContext = postsViewModel;
    }
}