﻿using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

namespace ShortageManager.ConsoleApp.Services.Authentication;

public interface IAuthenticator
{
    bool Login(string username);

    void Logout();

    void Register(string username, UserRole role);
}