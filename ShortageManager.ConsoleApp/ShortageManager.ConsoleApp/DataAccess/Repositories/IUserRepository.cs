using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public interface IUserRepository
{
    User? GetUser(string username);

    IEnumerable<User> GetUsers();

    void Add(User user);
}