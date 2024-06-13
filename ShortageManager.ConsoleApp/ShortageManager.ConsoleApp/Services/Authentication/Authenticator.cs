using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;

namespace ShortageManager.ConsoleApp.Services.Authentication;

public class Authenticator(IUserRepository userRepository) : IAuthenticator
{
    public bool Login(string? username)
    {
        if (string.IsNullOrEmpty(username)) return false;

        var user = userRepository.GetUser(username);
        return user != null;
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
        userRepository.Register(userToAdd);
    }

    public void Logout()
    {
        Console.WriteLine("Successfully logged out!");
    }
}