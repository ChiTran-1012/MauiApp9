using MauiApp9.Models;
using MauiApp9.Services;
using MauiApp9.ViewModels;

namespace MauiApp9;

public partial class App : Application
{
    private static DatabaseService? database;

    public static DatabaseService Database
    {
        get
        {
            if (database == null)
            {
                string dbName = "travel2.db";
                string targetPath = Path.Combine(FileSystem.AppDataDirectory, dbName);

                if (!File.Exists(targetPath))
                {
                    using var stream = FileSystem.OpenAppPackageFileAsync(dbName).Result;
                    using var fileStream = File.Create(targetPath);
                    stream.CopyTo(fileStream);
                }

                database = new DatabaseService(targetPath);
            }

            return database;
        }
    }

    public App()
    {
        InitializeComponent();

        // Bao MainPage trong NavigationPage để hỗ trợ chuyển trang (PushAsync)
        MainPage = new NavigationPage(new MainPage());
    }

    // Nếu bạn dùng .NET MAUI 9 trở lên, nên override CreateWindow như sau:
    /*
    protected override Window CreateWindow(IActivationState activationState)
    {
        return new Window(new NavigationPage(new MainPage()));
    }
    */
}
