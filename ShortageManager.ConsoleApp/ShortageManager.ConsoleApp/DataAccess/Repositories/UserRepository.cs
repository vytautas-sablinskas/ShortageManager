using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public class UserRepository(List<User> Users) : IUserRepository
{
    public User? GetUser(string username) => Users.FirstOrDefault(u => string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase));

    public IEnumerable<User> GetUsers() => Users;

    public void Add(User user) => Users.Add(user);
}