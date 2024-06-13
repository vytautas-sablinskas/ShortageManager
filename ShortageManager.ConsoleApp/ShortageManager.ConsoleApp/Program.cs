using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppControl;
using ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.Authentication;

namespace ShortageManager.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        IFileManager jsonFileManager = new JsonFileManager();
        var users = jsonFileManager.Read<User>(FilePaths.Users)
                                   .ToList();
        var shortages = jsonFileManager.Read<Shortage>(FilePaths.Shortages)
                                       .ToList();

        IUserRepository userRepository = new UserRepository(users, jsonFileManager);
        IShortageRepository shortageRepository = new ShortageRepository(shortages, jsonFileManager);
        IAuthenticator authenticator = new Authenticator(userRepository);
        var authenticatedAppActionFactory = new AuthenticatedAppActionFactory(shortageRepository);
        var unauthenticatedAppActionFactory = new UnauthenticatedAppActionFactory(authenticator, authenticatedAppActionFactory, userRepository);
        var appController = new AppController(unauthenticatedAppActionFactory);

        appController.RunShortageManagerApp();
    }
}