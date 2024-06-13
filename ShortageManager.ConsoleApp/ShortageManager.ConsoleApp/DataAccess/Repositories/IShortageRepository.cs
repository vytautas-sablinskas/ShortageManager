using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public interface IShortageRepository
{
    IEnumerable<Shortage> GetShortages();

    void Add(Shortage shortage);

    void Update(Shortage oldShortage, Shortage newShortage);
}