using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GameLibDesktop.ViewModels;

namespace GameLibDesktop.Views.UserControls;

public partial class GamesListUC : UserControl
{
    public GamesListUC()
    {
        InitializeComponent();
        DataContext = GamesListVM.GetInstance();
    }

    private void SelectingItemsControl_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        
    }
}