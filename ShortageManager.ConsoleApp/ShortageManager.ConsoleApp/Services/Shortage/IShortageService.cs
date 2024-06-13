using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

namespace ShortageManager.ConsoleApp.Services.ShortageService;

public interface IShortageService
{
    bool Delete(string title, RoomType room);

    bool Register(Shortage shortageToAdd);
}