using ShortageManager.ConsoleApp.DataAccess.Models;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public class UserRepository(List<User> users)
{
    public required List<User> Users { get; set; } = users;

    public void Add(User user) 
    { 
        if (Users.Contains(user))
        {
            Console.WriteLine($"User is by username: `{user.UserName}` is already registered!");
            return;
        }

        Users.Add(user);
    }


}