using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.AppSessionModel;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;

namespace ShortageManager.ConsoleApp.Services.ShortageService;

public class ShortageService(IShortageRepository shortageRepository, IUserRepository userRepository, IFileManager jsonFileManager) : IShortageService
{
    public bool Register(Shortage shortageToAdd)
    {
        var shortages = shortageRepository.GetShortages();

        if (!shortages.Contains(shortageToAdd))
        {
            shortageRepository.Add(shortageToAdd);
            jsonFileManager.Write(FilePaths.Shortages, shortages);
            return true;
        }

        var existingShortage = shortages.FirstOrDefault(shortageToAdd);
        if (existingShortage.Priority >= shortageToAdd.Priority)
        {
            return false;
        }

        shortageRepository.Update(existingShortage, shortageToAdd);
        jsonFileManager.Write(FilePaths.Shortages, shortages);

        return true;
    }

    public bool Delete(string title, RoomType roomType)
    {
        var shortages = shortageRepository.GetShortages();
        var existingShortage = shortages.FirstOrDefault(s => string.Equals(s.Title, title, StringComparison.OrdinalIgnoreCase) &&
                                                        s.Room == roomType);
        if (existingShortage == null)
        {
            return false;
        }

        var session = AppSession.Instance;
        var user = userRepository.GetUser(session.UserName);
        if (user == null)
        {
            return false;
        }

        if (existingShortage.Creator.UserName != user.UserName && 
            user.Role != UserRole.Administrator)
        {
            return false;
        }

        shortageRepository.Delete(existingShortage);
        jsonFileManager.Write(FilePaths.Shortages, shortages);

        return true;
    }
}
