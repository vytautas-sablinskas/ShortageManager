using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

namespace ShortageManager.ConsoleApp.Services.ShortageService;

public interface IShortageService
{
    bool Register(Shortage shortageToAdd);
}