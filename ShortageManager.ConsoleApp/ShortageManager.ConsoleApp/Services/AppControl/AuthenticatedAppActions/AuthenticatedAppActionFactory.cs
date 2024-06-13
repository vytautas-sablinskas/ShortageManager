
using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class AuthenticatedAppActionFactory(IShortageRepository shortageRepository)
{
    public IAppAction GetAction(string input)
    {
        return input switch
        {
            AuthenticatedActions.RegisterShortage => new RegisterShortageAction(shortageRepository),
            AuthenticatedActions.DeleteShortage => new ExitApplicationAction(),
            AuthenticatedActions.ListShortages => new ExitApplicationAction(),
            AuthenticatedActions.Logout => new LogoutAction(),
            _ => throw new ArgumentException("Invalid authenticated action input")
        };
    }
}