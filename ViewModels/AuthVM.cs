using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Windows.Input;
using PokemonsAPI.Models;
using ReactiveUI;

namespace GameLibDesktop.ViewModels;

public class AuthVM : ViewModelBase
{
    private static AuthVM Instance;
    private static string _PasswordText;
    private static string _LoginText;
    private static string _StatusText;
    private static bool _RememberMe;
    private static bool _ShowForAdmin;
    
    //Сеттеры закрытых полей, к которым происходит Binding
    public bool ShowForAdmin
    {
        get { return ShowForAdmin; }
        set { this.RaiseAndSetIfChanged(ref _ShowForAdmin, value); }
    }
    
    public string PasswordText
    {
        get { return _PasswordText; }
        set { this.RaiseAndSetIfChanged(ref _PasswordText, value); }
    }

    public string LoginText
    {
        get { return _LoginText; }
        set { this.RaiseAndSetIfChanged(ref _LoginText, value); }
    }

    public string StatusText
    {
        get { return _StatusText; }
        set { this.RaiseAndSetIfChanged(ref _StatusText, value); }
    }
    public static AuthVM GetInstance()
    {
        if (Instance == null)
        {
            Instance = new AuthVM();
        }

        Instance.LoginText = "Zombus";
        Instance.PasswordText = "12";

        Instance.StatusText = "";
        return Instance;
    }

    public ICommand EnterBtn()
    {
        string Token;
        string url = Program.HostAdress + "/api/ForAllUser/Login?login=" + LoginText + "&password=" + PasswordText;

        JsonDocument request;
        try
        {
            request = JsonDocument.Parse(Program.wc.UploadString(url, "POST", ""));
        }
        catch (Exception ex)
        {
            StatusText = "Неверный логин или пароль";
            return null;
        }

        Token = Convert.ToString(request.RootElement.GetProperty("accessToken").ToString());
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + Token);
        User CurrentUser = JsonSerializer.Deserialize<List<User>>(JsonDocument
            .Parse(Program.wc.DownloadString(
                    Program.HostAdress + "/api/ForAdmin/GetUsers")
            ))
            .Where(u => u.Login == LoginText && u.Password == Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(PasswordText)))
            .FirstOrDefault();
        
        //Проверка сохранённых данных входа
        if (MainWindowViewModel.config.AppSettings.Settings["AccessToken"]?.Value == null)
        {
            MainWindowViewModel.config.AppSettings.Settings.Add("AccessToken", request.RootElement.GetProperty("accessToken").ToString()); 
            MainWindowViewModel.config.AppSettings.Settings.Add("RefreshToken", request.RootElement.GetProperty("refreshToken").ToString());
            MainWindowViewModel.config.Save();
        }
        else
        {
            MainWindowViewModel.config.AppSettings.Settings.Remove("AccessToken");
            MainWindowViewModel.config.AppSettings.Settings.Remove("RefreshToken");
            MainWindowViewModel.config.AppSettings.Settings.Add("AccessToken", request.RootElement.GetProperty("accessToken").ToString());
            MainWindowViewModel.config.AppSettings.Settings.Add("RefreshToken", request.RootElement.GetProperty("refreshToken").ToString());
            MainWindowViewModel.config.Save();
        }
        
        Program.timer.Start();
        MainWindowViewModel.GetInstance().MenuIsVisible = true;
        MainWindowViewModel.GetInstance().menuVM = null;
        MainWindowViewModel.GetInstance().menuVM = MenuVM.GetInstance();
        MainWindowViewModel.GetInstance().CurrentControl = GamesListVM.GetInstance();

        return null;
    }
    
    private AuthVM()
    {}
}