namespace ShortageManager.ConsoleApp.DataAccess.Models.AppSessionModel;

public class AppSession
{
    private static readonly AppSession _instance = new AppSession();

    private AppSession() { }

    public static AppSession Instance => _instance;

    public string UserName { get; set; } = "";
}