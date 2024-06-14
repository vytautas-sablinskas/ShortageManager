using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppSessionService;
using ShortageManager.ConsoleApp.Services.ShortageFilter;

namespace ShortageManager.ConsoleApp.Services.ShortageService;

public class ShortageService(IShortageRepository shortageRepository, IUserRepository userRepository, IFileManager jsonFileManager) : IShortageService
{
    public IEnumerable<Shortage> GetShortagesByPermissionsAndFilters(Dictionary<ShortageFilterType, List<string>> selectedFilters)
    {
        var user = userRepository.GetUser(AppSession.UserName);
        if (user == null)
        {
            throw new InvalidOperationException("User trying to access shortages was not found in database!");
        }

        var filteredShortages = shortageRepository.GetShortagesByPermissionsAndFilters(user, selectedFilters);

        return filteredShortages;
    }

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

        var user = userRepository.GetUser(AppSession.UserName);
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