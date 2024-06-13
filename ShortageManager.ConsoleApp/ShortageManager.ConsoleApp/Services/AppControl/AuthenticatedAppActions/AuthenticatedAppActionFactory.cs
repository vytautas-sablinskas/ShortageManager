
using ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class AuthenticatedAppActionFactory
{
    public IAppAction GetAction(string input)
    {
        return input switch
        {
            "1" => new ExitApplicationAction(),
            "2" => new ExitApplicationAction(),
            "3" => new ExitApplicationAction(),
            "4" => new LogoutAction(),
            _ => throw new ArgumentException("Invalid action input")
        };
    }
}
