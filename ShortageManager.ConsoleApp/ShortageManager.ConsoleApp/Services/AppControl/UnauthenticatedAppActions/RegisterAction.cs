﻿using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.Authentication;

namespace ShortageManager.ConsoleApp.Services.AppControl.UnauthenticatedAppActions;

public class RegisterAction(IUserRepository userRepository, IUserService authenticator) : IAppAction
{
    public void Execute()
    {
        const string AdministratorRole = "1";

        Console.Clear();

        var isUserRegistered = false;
        while (!isUserRegistered)
        {
            Console.WriteLine("Press enter without typing anything to go back to menu.");
            Console.WriteLine("Enter your username:");
            var usernameToRegister = Console.ReadLine()?
                                            .Trim();
            if (string.IsNullOrEmpty(usernameToRegister))
            {
                Console.Clear();
                return;
            }

            var user = userRepository.GetUser(usernameToRegister);
            if (user != null)
            {
                Console.Clear();
                Console.WriteLine($"User with username '{usernameToRegister}' is already registered. Try another username!\n");
                continue;
            }

            Console.WriteLine("Enter user role (1 - administrator, any other letter or number - simple user)");
            var selectedRole = Console.ReadLine()?
                                           .Trim();
            var userRole = selectedRole == AdministratorRole ? UserRole.Administrator : UserRole.User;

            authenticator.Register(usernameToRegister, userRole);

            Console.Clear();
            Console.WriteLine($"User with username '{usernameToRegister}' was successfully registered!\n");

            isUserRegistered = true;
        }
    }
}