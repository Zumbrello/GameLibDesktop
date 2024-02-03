using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using GameLibDesktop.ViewModels;

namespace GameLibDesktop;

sealed class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    public static WebClient wc = new WebClient(); 
    public static string HostAdress = "http://5.144.96.227:5555";
    //public static string HostAdress = "http://localhost:5269";
    public static System.Timers.Timer timer = new System.Timers.Timer();

    [STAThread]
    public static void Main(string[] args)
    {
        timer.Interval = 1000 * 60 * 50;
        timer.Elapsed += timer_Elapsed;
        timer.AutoReset = true;
        BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
    }
    
    static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    {
        UpdateTokens();
    }

    public static void UpdateTokens()
    {
        string url = HostAdress + "/Login/UpdateTokens";

        try
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["TokenRefresh"].Value);
    
            var response = client.Send(request);
            response.EnsureSuccessStatusCode();
            JsonDocument jsonResponse = JsonDocument.Parse(new StreamReader(response.Content.ReadAsStream()).ReadToEnd());

            if (jsonResponse.RootElement.GetProperty("error").ToString() != "null")
            {
                throw new Exception("");
            }
            
            MainWindowViewModel.config.AppSettings.Settings.Remove("Token");
            MainWindowViewModel.config.AppSettings.Settings.Remove("TokenRefresh");
            MainWindowViewModel.config.AppSettings.Settings.Add("Token",
                Convert.ToString(jsonResponse.RootElement.GetProperty("token").ToString()));
            MainWindowViewModel.config.AppSettings.Settings.Add("TokenRefresh",
                Convert.ToString(jsonResponse.RootElement.GetProperty("refreshToken").ToString()));
            MainWindowViewModel.config.Save();
            wc.Headers.Clear();
            wc.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["Token"].Value);
            
            url = HostAdress + "/GetUserById?id=" + jsonResponse.RootElement.GetProperty("id");
            request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Authorization", "Bearer " + MainWindowViewModel.config.AppSettings.Settings["Token"].Value);
    
            response = client.Send(request);
            response.EnsureSuccessStatusCode();
            jsonResponse = JsonDocument.Parse(new StreamReader(response.Content.ReadAsStream()).ReadToEnd());

            if (jsonResponse.RootElement.GetProperty("error").ToString() != "null")
            {
                throw new Exception("");
            }
        }
        catch (Exception ex)
        {
            MainWindowViewModel.config.AppSettings.Settings.Remove("TokenRefresh");
            MainWindowViewModel.config.AppSettings.Settings.Remove("Token");
            MainWindowViewModel.config.Save();
            MainWindowViewModel.GetInstance().CurrentControl = AuthVM.GetInstance();
            MainWindowViewModel.GetInstance().MenuIsVisible = false;
        }
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
}