using System;
using System.Windows.Input;

namespace GameLibDesktop.ViewModels;

public class MenuVM : ViewModelBase
{
    private static MenuVM Instance;

    public static MenuVM GetInstance()
    {
        if (Instance == null)
        {
            Instance = new MenuVM();
        }

        return Instance;
    }
    public ICommand Catalog()
    {
        MainWindowViewModel.GetInstance().CurrentControl = null;
        MainWindowViewModel.GetInstance().CurrentControl = GamesListVM.GetInstance();
        return null;
    }
    public ICommand Users()
    {
        MainWindowViewModel.GetInstance().CurrentControl = null;
        MainWindowViewModel.GetInstance().CurrentControl = ChangeUserVM.GetInstance();
        return null;
    }
    public ICommand AddGame()
    {
        MainWindowViewModel.GetInstance().CurrentControl = null;
        MainWindowViewModel.GetInstance().CurrentControl = AddGameVM.GetInstance();
        return null;
    }

    public ICommand Logout()
    {
        MainWindowViewModel.GetInstance().CurrentControl = null;
        MainWindowViewModel.GetInstance().CurrentControl = AuthVM.GetInstance();
        MainWindowViewModel.GetInstance().MenuIsVisible = false;
        MainWindowViewModel.GetInstance().menuVM = null;
        return null;
    }
    
    public ICommand Exit()
    {
        Environment.Exit(0);
        return null;
    }
    private MenuVM()
    {
        
    }
}