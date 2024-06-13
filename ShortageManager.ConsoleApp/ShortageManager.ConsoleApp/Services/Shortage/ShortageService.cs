using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;

namespace ShortageManager.ConsoleApp.Services.ShortageService;

public class ShortageService(IShortageRepository shortageRepository, IFileManager jsonFileManager) : IShortageService
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
}
