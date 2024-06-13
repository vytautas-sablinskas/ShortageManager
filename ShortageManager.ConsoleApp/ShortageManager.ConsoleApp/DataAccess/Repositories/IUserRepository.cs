using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public interface IUserRepository
{
    void Register(User user);

    User? GetUser(string username);
}