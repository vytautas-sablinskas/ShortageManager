﻿using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

namespace ShortageManager.ConsoleApp.Services.AppControl;

public class AppController(UnauthenticatedAppActionFactory unauthenticatedAppActionFactory) : IAppController
{
    private readonly List<string> _validInputs = new()
    {
        UnauthenticatedActions.Login,
        UnauthenticatedActions.Register,
        UnauthenticatedActions.ExitApp
    };

    public void RunShortageManagerApp()
    {
        while (true)
        {
            Console.WriteLine(NavigationMessages.StartingPageMessage);
            var input = Console.ReadLine()?
                               .Trim();
            var action = GetUserSelectedAction(input);
            if (action == null)
            {
                continue;
            }

            action.Execute();
        }
    }

    public IAppAction? GetUserSelectedAction(string? input)
    {
        if (string.IsNullOrEmpty(input) || !_validInputs.Contains(input))
        {
            Console.Clear();
            Console.WriteLine("Invalid choice was given. Try again!\n");

            return null;
        }

        return unauthenticatedAppActionFactory.GetAction(input);
    }
}