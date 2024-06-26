﻿using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppControl;
using ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;
using ShortageManager.ConsoleApp.Services.Authentication;
using ShortageManager.ConsoleApp.Services.ShortageService;

namespace ShortageManager.ConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        SetupDataFolder();

        var appController = InitializeAppController();
        appController.RunShortageManagerApp();
    }

    private static void SetupDataFolder()
    {
        if (!File.Exists(FilePaths.DataFolder))
        {
            Directory.CreateDirectory(FilePaths.DataFolder);
        }
    }

    private static AppController InitializeAppController()
    {
        IFileManager jsonFileManager = new JsonFileManager();

        var users = jsonFileManager.Read<User>(FilePaths.Users)
                                   .ToList();
        var shortages = jsonFileManager.Read<Shortage>(FilePaths.Shortages)
                                       .ToList();

        IUserRepository userRepository = new UserRepository(users);
        IShortageRepository shortageRepository = new ShortageRepository(shortages);

        IUserService userService = new UserService(userRepository, jsonFileManager);
        IShortageService shortageService = new ShortageService(shortageRepository, userRepository, jsonFileManager);

        var authenticatedAppActionFactory = new AuthenticatedAppActionFactory(shortageService, userRepository);
        var unauthenticatedAppActionFactory = new UnauthenticatedAppActionFactory(userService, authenticatedAppActionFactory, userRepository);

        return new AppController(unauthenticatedAppActionFactory);
    }
}