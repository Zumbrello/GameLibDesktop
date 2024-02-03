using System;
using System.Configuration;
using ReactiveUI;

namespace GameLibDesktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public static Configuration config = ConfigurationManager.OpenExeConfiguration(AppDomain.CurrentDomain.BaseDirectory + "GameLibDesktop");
    private static MainWindowViewModel Instance;
    private MenuVM _menuVM;
    private ViewModelBase _CurrentContent;
    private bool _MenuIsVisible;

    public static MainWindowViewModel GetInstance()
    {
        if (Instance == null)
        {
            Instance = new MainWindowViewModel();
        }

        return Instance;
    }

    public MenuVM menuVM
    {
        get { return _menuVM; }
        set
        {
            _menuVM = value;
            this.RaisePropertyChanged();
        }
    }

    public ViewModelBase CurrentControl
    {
        get { return _CurrentContent; }
        set
        {
            _CurrentContent = value;
            this.RaisePropertyChanged();
        }
    }

    public bool MenuIsVisible
    {
        get { return _MenuIsVisible; }
        set
        {
            _MenuIsVisible = value;
            this.RaisePropertyChanged();
        }

    }
    
    private MainWindowViewModel()
    {
        MenuIsVisible = false;
        menuVM = MenuVM.GetInstance();
        CurrentControl = AuthVM.GetInstance();
    }
}