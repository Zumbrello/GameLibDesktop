using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reactive.Linq;
using System.Text.Json;
using System.Threading;
using Avalonia.Media.Imaging;
using DynamicData;
using PokemonsAPI.Models;
using ReactiveUI;

namespace GameLibDesktop.ViewModels;

public class GamesListVM : ViewModelBase
{
    private static GamesListVM Instance;
    private ObservableCollection<GameCard> _GamesList;
    private ObservableCollection<Publisher> _PublishersList;
    private ObservableCollection<Developer> _DevelopersList;
    private List<string> _DevelopersCBList;
    private List<string> _PublishersCBList;
    private string _SelectedPublisher;
    private string _SelectedDeveloper;
    private string _SearchText;

    public string SearchText
    {
        get { return _SearchText; }
        set
        {
            _SearchText = value;
            this.RaisePropertyChanged();
            FillList();
        }
    }

    public string SelectedPublisher
    {
        get { return _SelectedPublisher; }
        set
        {
            _SelectedPublisher = value;
            this.RaisePropertyChanged();
            FillList();
        }
    }
    public string SelectedDeveloper
    {
        get { return _SelectedDeveloper; }
        set
        {
            _SelectedDeveloper = value;
            this.RaisePropertyChanged();
            FillList();
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
    
    public ObservableCollection<GameCard> GamesList
    {
        get { return _GamesList; }
        set
        {
            _GamesList = value;
            this.RaisePropertyChanged();
        }
    }
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
    public static GamesListVM GetInstance()
    {
        if (Instance == null)
        {
            Instance = new GamesListVM();
        }
        
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        Instance.PublishersList = new ObservableCollection<Publisher>();
        Instance.DevelopersList = new ObservableCollection<Developer>();
        Instance.PublishersCBList = new List<string>(){"Все издатели"};
        Instance.DevelopersCBList = new List<string>(){"Все разработчики"};

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
        Instance.FillList();
        return Instance;
    }

    private GamesListVM()
    {}

    private void FillList()
    {
        try
        {
            GamesList = new ObservableCollection<GameCard>();
            Program.wc.Headers.Clear();
            Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
            var resultGames = Program.wc.DownloadString(Program.HostAdress + "/api/ForAllUser/GetGame");
            var gamesList = JsonSerializer.Deserialize<List<Game>>(resultGames);

            if (SelectedPublisher != "Все издатели" && SelectedPublisher != null)
            {
                gamesList = gamesList.Where(g =>
                    g.IdPublisher == PublishersList.Where(p => p.Publisher1 == SelectedPublisher).First().Id).ToList();
            }
            
            if (SelectedDeveloper != "Все разработчики" && SelectedDeveloper != null)
            {
                gamesList = gamesList.Where(g =>
                    g.IdPublisher == DevelopersList.Where(p => p.Developer1 == SelectedDeveloper).First().Id).ToList();
            }
            
            if (SearchText != "" && SearchText != null)
            {
                gamesList = gamesList.Where(g => g.GameName.Contains(SearchText)).ToList();
            }
            
            foreach (var game in gamesList)
            {
                try
                {
                    GamesList.Add(new GameCard()
                    {

                        GameName = game.GameName, Description = game.Description,
                        Developer = "Разработчик: " +
                                    DevelopersList.Where(p => p.Id == game.IdDeveloper).First().Developer1,
                        Publisher = "Издатель: " +
                                    PublishersList.Where(p => p.Id == game.IdPublisher).First().Publisher1,
                        MainImage = new Bitmap(new MemoryStream(Program.wc.DownloadData(game.MainImage))),
                        ReleaseDate = game.ReleaseDate,
                        SystemRequestMin = game.SystemRequestMin, SystemRequestRec = game.SystemRequestRec
                    });
                }
                catch (Exception e)
                {
                    
                }
            }
            
        }
        catch (Exception ex)
        {
            GamesList = new ObservableCollection<GameCard>();
        }
    }
}