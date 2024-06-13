using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.Authentication;

namespace ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

public class LoginAction(IUserService userService, AuthenticatedAppActionFactory authenticatedAppActionFactory) : IAppAction
{
    private readonly List<string> _validAuthenticatedInputs = new() 
    { 
        AuthenticatedActions.RegisterShortage, 
        AuthenticatedActions.DeleteShortage, 
        AuthenticatedActions.ListShortages, 
        AuthenticatedActions.Logout 
    };

    public void Execute()
    {
        Console.Clear();

        while (true)
        {
            Console.WriteLine("Press enter without typing anything to go back to menu.");
            Console.WriteLine("Enter your username:");
            var userToLogin = Console.ReadLine()?
                                     .Trim();
            if (string.IsNullOrEmpty(userToLogin))
            {
                Console.Clear();
                return;
            }

            var isLoginSuccessful = userService.Login(userToLogin);
            if (!isLoginSuccessful)
            {
                Console.Clear();
                Console.WriteLine($"User by username '{userToLogin}' does not exist. Try again!\n");
                continue;
            }

            break;
        }

        ShowAuthenticatedUserMenu();
    }

    public void ShowAuthenticatedUserMenu()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine(NavigationMessages.AuthenticatedMainPageMessage);
            var input = Console.ReadLine()?
                               .Trim();
            if (string.IsNullOrEmpty(input) || !_validAuthenticatedInputs.Contains(input))
            {
                continue;
            }

            var action = authenticatedAppActionFactory.GetAction(input);
            action.Execute();

            if (action is LogoutAction)
            {
                return;
            }
        }
    }
}
