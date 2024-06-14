using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.Services.ShortageFilter;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public interface IShortageRepository
{
    IEnumerable<Shortage> GetShortages();

    IEnumerable<Shortage> GetShortagesByPermissionsAndFilters(User user, Dictionary<ShortageFilterType, List<string>> filters);

    void Add(Shortage shortage);

    void Delete(Shortage shortage);

    void Update(Shortage oldShortage, Shortage newShortage);
}