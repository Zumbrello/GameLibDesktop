using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GameLibDesktop.ViewModels;

namespace GameLibDesktop.Views.UserControls;

public partial class AddGameUC : UserControl
{
    public AddGameUC()
    {
        InitializeComponent();
        DataContext = AddGameVM.GetInstance(0);
    }
}