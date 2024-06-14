using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.Authentication;

namespace ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

public class UnauthenticatedAppActionFactory(
    IUserService userService,
    AuthenticatedAppActionFactory authenticatedAppActionFactory,
    IUserRepository userRepository)
{
    public IAppAction GetAction(string input)
    {
        return input switch
        {
            UnauthenticatedActions.Login => new LoginAction(userService, authenticatedAppActionFactory),
            UnauthenticatedActions.Register => new RegisterAction(userRepository, userService),
            UnauthenticatedActions.ExitApp => new ExitApplicationAction(),
            _ => throw new ArgumentException("Invalid unauthenticated action input")
        };
    }
}