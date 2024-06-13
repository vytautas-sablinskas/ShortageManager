namespace ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

public class ExitApplicationAction : IAppAction
{
    public void Execute()
    {
        Console.Clear();
        Console.WriteLine("App is closing!");
        Environment.Exit(0);
    }
}