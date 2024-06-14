using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.ShortageService;

namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class AuthenticatedAppActionFactory(IShortageService shortageService, IUserRepository userRepository)
{
    public IAppAction GetAction(string input)
    {
        return input switch
        {
            AuthenticatedActions.RegisterShortage => new RegisterShortageAction(shortageService, userRepository),
            AuthenticatedActions.DeleteShortage => new DeleteShortageAction(shortageService),
            AuthenticatedActions.ListShortages => new ListShortagesAction(shortageService),
            AuthenticatedActions.Logout => new LogoutAction(),
            _ => throw new ArgumentException("Invalid authenticated action input")
        };
    }
}