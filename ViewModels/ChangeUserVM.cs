using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using GameLibDesktop.Models;
using ReactiveUI;

namespace GameLibDesktop.ViewModels;

public class ChangeUserVM : ViewModelBase
{
    private static ChangeUserVM Instance;
    private string _LoginText;
    private string _EmailText;
    private string _PasswordText;
    private string _StatusText;
    private User currentUser;

    public string LoginText
    {
        get { return _LoginText; }
        set
        {
            _LoginText = value;
            this.RaisePropertyChanged();
        }
    }
    public string EmailText
    {
        get { return _EmailText; }
        set
        {
            _EmailText = value;
            this.RaisePropertyChanged();
        }
    }
    public string PasswordText
    {
        get { return _PasswordText; }
        set
        {
            _PasswordText = value;
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
    public static ChangeUserVM GetInstance()
    {
        if (Instance == null)
        {
            Instance = new ChangeUserVM();
        }

        return Instance;
    }

    public ICommand FindUser()
    {
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        var resultUsers = Program.wc.DownloadString(Program.HostAdress + "/api/ForAdmin/GetUsers");
        var usersList = JsonSerializer.Deserialize<List<User>>(resultUsers);

        currentUser = usersList.Where(u => u.Login == LoginText).FirstOrDefault();
        if (currentUser == null)
        {
            StatusText = "Нет пользователя с таким логином!";
            EmailText = "";
            PasswordText = "";
            
            HideStatus();
            return null;
        }
        else
        {
            StatusText = "Пользователь найден !";
        }

        EmailText = currentUser.Email;
        PasswordText = System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(currentUser.Password));
        
        HideStatus();
        return null;
    }
    
    public ICommand ChangeUser()
    {
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        try
        {
            var resultUsers = Program.wc.UploadString(
                Program.HostAdress + $"/api/ForAllUser/ChangeUser?" +
                $"login={LoginText}" +
                $"&email={EmailText}" +
                $"&password={System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(PasswordText))}" +
                $"&id={currentUser.Id}", "PUT",
                "");
            StatusText = "Данные успешно обновлены";
        }
        catch (Exception ex)
        {
            StatusText = "Ошибка обновления данных";
        }
        return null;
    }
    
    public ICommand DeleteUser()
    {
        
        Program.wc.Headers.Clear();
        Program.wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["AccessToken"].Value);
        try
        {
            var resultUsers = Program.wc.UploadString(
                Program.HostAdress + $"/api/ForAdmin/DeleteUser?" +
                $"id_user={currentUser.Id}", "DELETE", "");
            
            StatusText = "Пользователь успешно удалён";
        }
        catch (Exception ex)
        {
            StatusText = "Ошибка удаления пользователя";
        }

        return null;
    }
    
    private ChangeUserVM()
    {
        
    }
    
    private async void HideStatus()
    {
        await Task.Delay(5000);
        StatusText = "";
    }
}