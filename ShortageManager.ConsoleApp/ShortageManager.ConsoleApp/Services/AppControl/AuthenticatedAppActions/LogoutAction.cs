namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class LogoutAction : IAppAction
{
    public void Execute()
    {
        Console.Clear();
        Console.WriteLine("Successfully logged out!\n");
    }
}