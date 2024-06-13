using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public class UserRepository(List<User> Users, IFileManager jsonFileManager) : IUserRepository
{
    public void Register(User user) 
    { 
        if (Users.Contains(user))
        {
            Console.WriteLine($"User by username: '{user.UserName}' is already registered!");
            return;
        }

        Users.Add(user);
        jsonFileManager.Append(FilePaths.Users, Users);
    }

    public User? GetUser(string username) => Users.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase));
}