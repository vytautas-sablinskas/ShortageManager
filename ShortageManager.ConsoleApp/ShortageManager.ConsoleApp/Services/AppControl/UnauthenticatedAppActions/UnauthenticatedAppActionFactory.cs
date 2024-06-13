using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.Authentication;

namespace ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

public class UnauthenticatedAppActionFactory(
    IAuthenticator authenticator, 
    AuthenticatedAppActionFactory authenticatedAppActionFactory, 
    IUserRepository userRepository)
{
    public IAppAction GetAction(string input)
    {
        return input switch
        {
            "1" => new LoginAction(authenticator, authenticatedAppActionFactory),
            "2" => new RegisterAction(userRepository),
            "3" => new ExitApplicationAction(),
            _ => throw new ArgumentException("Invalid action input")
        };
    }
}