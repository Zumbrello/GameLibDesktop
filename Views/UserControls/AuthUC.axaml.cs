using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GameLibDesktop.ViewModels;

namespace GameLibDesktop.Views.UserControls;

public partial class AuthUC : UserControl
{
    public AuthUC()
    {
        InitializeComponent();
        DataContext = AuthVM.GetInstance();
    }
}