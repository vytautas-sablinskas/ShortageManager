using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public class ShortageRepository(List<Shortage> shortages) : IShortageRepository
{
    public IEnumerable<Shortage> GetShortages() => shortages;

    public void Add(Shortage shortage) => shortages.Add(shortage);

    public void Update(Shortage oldShortage, Shortage newShortage)
    {
        shortages.Remove(oldShortage);
        shortages.Add(newShortage);
    }
}