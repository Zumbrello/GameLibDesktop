using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Windows.Input;
using GameLibDesktop.Models;
using ReactiveUI;

namespace GameLibDesktop.ViewModels;

public class AddGameVM : ViewModelBase
{
    private static AddGameVM Instance;
    private string _TitleText;
    private string _StatusText;
    private string _DescriptionText;
    private string _DeveloperText;
    private string _PublisherText;
    private string _MinReqText;
    private string _MaxReqText;
    private string _ImageUrlText;
    private string _ReleaseDateText;
    private List<string> _DevelopersCBList;
    private List<string> _PublishersCBList;
    private string _SelectedPublisher;
    private string _SelectedDeveloper;
    private ObservableCollection<Publisher> _PublishersList;
    private ObservableCollection<Developer> _DevelopersList;
    private bool _addBtnVisible;
    private bool _changeBtnVisible;
    private bool _deleteBtnVisible;
    private int _mode;
    private GameCard _game;


    public ObservableCollection<Publisher> PublishersList
    {
        get { return _PublishersList; }
        set
        {
            _PublishersList = value;
            this.RaisePropertyChanged();
        }
    }
    public ObservableCollection<Developer> DevelopersList
    {
        get { return _DevelopersList; }
        set
        {
            _DevelopersList = value;
            this.RaisePropertyChanged();
        }
    }
    public string SelectedPublisher
    {
        get { return _SelectedPublisher; }
        set
        {
            _SelectedPublisher = value;
            this.RaisePropertyChanged();
        }
    }
    public string SelectedDeveloper
    {
        get { return _SelectedDeveloper; }
        set
        {
            _SelectedDeveloper = value;
            this.RaisePropertyChanged();
        }
    }
    public List<string> DevelopersCBList
    {
        get { return _DevelopersCBList; }
        set
        {
            _DevelopersCBList = value;
            this.RaisePropertyChanged();
        }
    }
    
    public List<string> PublishersCBList
    {
        get { return _PublishersCBList; }
        set
        {
            _PublishersCBList = value;
            this.RaisePropertyChanged();
        }
    }

    public static AddGameVM GetInstance(int mode, GameCard game = null)
    {
        // instance - экземпляр окна, который сохраняет данные при переходе между окнами
        if (Instance == null)
        {
            Instance = new AddGameVM();
        }
        
        Instance._mode = mode;
        // если mod == 1, идет изменение игры. если mod == 0, то добавление игры
        if (mode == 1)
        {
            Instance.TitleText = game.GameName;
            Instance.DescriptionText = game.Description;
            Instance.ImageUrlText = game.ImageURl;
            Instance.ReleaseDateText = game.ReleaseDate;
            Instance.MinReqText = game.SystemRequestMin;
            Instance.MaxReqText = game.SystemRequestRec;
            Instance.SelectedDeveloper = game.Developer;
            Instance.SelectedPublisher = game.Publisher;
            Instance.AddBtnVisible = false;
            Instance.ChangeBtnVisible = true;
            Instance.DeleteBtnVisible = true;
            Instance._game = game;
        }
        else
        {
            Instance.AddBtnVisible = true;
            Instance.ChangeBtnVisible = false;
            Instance.DeleteBtnVisible = false;
        }
        Instance.ReFillLists();
        return Instance;
    }
    public string TitleText
    {
        get { return _TitleText; }
        set
        {
            _TitleText = value;
            this.RaisePropertyChanged();
        }
    }
    public string DeveloperText
    {
        get { return _DeveloperText; }
        set
        {
            _DeveloperText = value;
            this.RaisePropertyChanged();
        }
    }
    public string PublisherText
    {
        get { return _PublisherText; }
        set
        {
            _PublisherText = value;
            this.RaisePropertyChanged();
        }
    }
    public string MinReqText
    {
        get { return _MinReqText; }
        set
        {
            _MinReqText = value;
            this.RaisePropertyChanged();
        }
    }
    public string MaxReqText
    {
        get { return _MaxReqText; }
        set
        {
            _MaxReqText = value;
            this.RaisePropertyChanged();
        }
    }
    public string ReleaseDateText
    {
        get { return _ReleaseDateText; }
        set
        {
            _ReleaseDateText = value;
            this.RaisePropertyChanged();
        }
    }
    public string ImageUrlText
    {
        get { return _ImageUrlText; }
        set
        {
            _ImageUrlText = value;
            this.RaisePropertyChanged();
        }
    }
    public string DescriptionText
    {
        get { return _DescriptionText; }
        set
        {
            _DescriptionText = value;
            this.RaisePropertyChanged();
        }
    }
    public string StatusText
    {
        get { return _StatusText; }
        set
        {
            _StatusText = value;
            this.RaisePropertyChanged();
        }
    }

    public bool AddBtnVisible
    {
        get { return _addBtnVisible; }
        set
        {
            _addBtnVisible = value;
            this.RaisePropertyChanged();
        }
    }
    
    public bool ChangeBtnVisible
    {
        get { return _changeBtnVisible; }
        set
        {
            _changeBtnVisible = value;
            this.RaisePropertyChanged();
        }
    }
    
    public bool DeleteBtnVisible
    {
        get { return _deleteBtnVisible; }
        set
        {
            _deleteBtnVisible = value;
            this.RaisePropertyChanged();
        }
    }

    public ICommand AddGame()
    {
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        AddGame newGame = new AddGame();
        
        newGame.Name = TitleText;
        newGame.IdDeveloper = DevelopersList.Where(d => d.Developer1 == SelectedDeveloper).First().Id;
        newGame.IdPublisher = PublishersList.Where(p => p.Publisher1 == SelectedPublisher).First().Id;
        newGame.Description = DescriptionText;
        newGame.MainImage = ImageUrlText;
        newGame.ReleaseDate = ReleaseDateText;
        newGame.SystemRequestMin = MinReqText;
        newGame.SystemRequestRec = MaxReqText;
        
        Program.wc.Headers[HttpRequestHeader.ContentType] = "application/json";
        try
        {
            Program.wc.UploadString(Program.HostAdress + "/api/ForAdmin/AddGame", "POST",
                JsonSerializer.Serialize(newGame));
            StatusText = "Игра успешно добавлена";
        }
        catch (Exception ex)
        {
            StatusText = "Ошибка при добавлении игры";
        }

        Instance = new AddGameVM();
        MainWindowViewModel.GetInstance().CurrentControl = GamesListVM.GetInstance();
        return null;
    }

    public ICommand ChangeGame()
    {
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        AddGame newChangeGame = new AddGame();
        
        newChangeGame.Name = TitleText;
        newChangeGame.IdDeveloper = DevelopersList.Where(d => d.Developer1 == SelectedDeveloper).First().Id;
        newChangeGame.IdPublisher = PublishersList.Where(p => p.Publisher1 == SelectedPublisher).First().Id;
        newChangeGame.Description = DescriptionText;
        newChangeGame.MainImage = ImageUrlText;
        newChangeGame.ReleaseDate = ReleaseDateText;
        newChangeGame.SystemRequestMin = MinReqText;
        newChangeGame.SystemRequestRec = MaxReqText;
        
        Program.wc.Headers[HttpRequestHeader.ContentType] = "application/json";
        try
        {
            Program.wc.UploadString(Program.HostAdress + "/api/ForAdmin/editgame", "POST",
                JsonSerializer.Serialize(newChangeGame));
            StatusText = "Игра успешно редактирована";
        }
        catch (Exception ex)
        {
            StatusText = "Ошибка при редактировании игры";
        }

        Instance = new AddGameVM();
        MainWindowViewModel.GetInstance().CurrentControl = GamesListVM.GetInstance();
        return null;
    }
    
    public ICommand DeleteGame()
    {
        //Запрос удаления игры
        return null;
    }
    
    private AddGameVM()
    {
        
    }

    private void ReFillLists()
    {
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        Instance.PublishersList = new ObservableCollection<Publisher>();
        Instance.DevelopersList = new ObservableCollection<Developer>();
        Instance.PublishersCBList = new List<string>();
        Instance.DevelopersCBList = new List<string>();

        var resultDevelopers = Program.wc.DownloadString( Program.HostAdress + "/api/ForAllUser/GetDevelopers");
        var developersList = JsonSerializer.Deserialize<List<Developer>>(resultDevelopers);
            
        var resultPublishers = Program.wc.DownloadString(Program.HostAdress + "/api/ForAllUser/GetPublisher");
        var publishersList = JsonSerializer.Deserialize<List<Publisher>>(resultPublishers);

        foreach (var publisher in publishersList)
        {
            Instance.PublishersList.Add(publisher);
            Instance.PublishersCBList.Add(publisher.Publisher1);
        }

        foreach (var developer in developersList)
        {
            Instance.DevelopersList.Add(developer);
            Instance.DevelopersCBList.Add(developer.Developer1);
        }
    }
}