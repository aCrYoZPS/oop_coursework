using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wonderlust.UI.ViewModels;

namespace Wonderlust.UI.Pages;

public partial class Communities : ContentPage
{
    public Communities(CommunitiesViewModel communitiesViewModel)
    {
        InitializeComponent();
        BindingContext = communitiesViewModel;
    }
}