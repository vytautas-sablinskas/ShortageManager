using ShortageManager.ConsoleApp.Constants;
using ShortageManager.ConsoleApp.DataAccess.InOut;
using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public class ShortageRepository(List<Shortage> Shortages, IFileManager jsonFileManager) : IShortageRepository
{
    public bool Register(Shortage shortageToAdd)
    {
        if (!Shortages.Contains(shortageToAdd))
        {
            Shortages.Add(shortageToAdd);
            jsonFileManager.Write(FilePaths.Shortages, Shortages);
            return true;
        }

        var existingShortage = Shortages.FirstOrDefault(shortageToAdd);
        if (existingShortage.Priority >= shortageToAdd.Priority)
        {
            return false;
        }

        Shortages.Remove(existingShortage);
        Shortages.Add(shortageToAdd);

        jsonFileManager.Write(FilePaths.Shortages, Shortages);

        return true;
    }
}