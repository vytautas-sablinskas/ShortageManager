using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.Services.ShortageFilter;

namespace ShortageManager.ConsoleApp.Services.ShortageService;

public interface IShortageService
{
    bool Delete(string title, RoomType room);

    bool Register(Shortage shortageToAdd);

    IEnumerable<Shortage> GetShortagesByPermissionsAndFilters(Dictionary<ShortageFilterType, List<string>> selectedFilters);
}