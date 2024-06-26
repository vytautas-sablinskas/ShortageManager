﻿using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppSessionService;

namespace ShortageManager.ConsoleApp.Services.Authentication;

public class UserService(IUserRepository userRepository, IFileManager jsonFileManager) : IUserService
{
    public bool Login(string? username)
    {
        if (string.IsNullOrEmpty(username)) return false;

        var user = userRepository.GetUser(username);
        if (user == null)
        {
            return false;
        }

        AppSession.UserName = username;
        return true;
    }

    public void Register(string username, UserRole role)
    {
        var user = userRepository.GetUser(username);
        if (user != null)
        {
            Console.WriteLine($"User by username '{username}' is already registered!");
            return;
        }

        var userToAdd = new User(username, role);
        var users = userRepository.GetUsers();

        if (users.Contains(user))
        {
            Console.WriteLine($"User by username: '{username}' is already registered!");
            return;
        }

        userRepository.Add(userToAdd);
        jsonFileManager.Write(FilePaths.Users, users);
    }

    public void Logout()
    {
        AppSession.UserName = "";
        Console.WriteLine("Successfully logged out!");
    }
}