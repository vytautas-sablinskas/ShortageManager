using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public interface IShortageRepository
{
    bool Register(Shortage shortageToAdd);
}