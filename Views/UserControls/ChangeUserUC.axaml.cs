using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GameLibDesktop.ViewModels;

namespace GameLibDesktop.Views.UserControls;

public partial class ChangeUserUC : UserControl
{
    public ChangeUserUC()
    {
        InitializeComponent();
        DataContext = ChangeUserVM.GetInstance();
    }
}