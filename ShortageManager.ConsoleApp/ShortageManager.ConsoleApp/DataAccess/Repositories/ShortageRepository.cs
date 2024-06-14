using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;
using ShortageManager.ConsoleApp.Services.ShortageFilter;

namespace ShortageManager.ConsoleApp.DataAccess.Repositories;

public class ShortageRepository(List<Shortage> shortages) : IShortageRepository
{
    public IEnumerable<Shortage> GetShortages() => shortages;

    public IEnumerable<Shortage> GetShortagesByPermissionsAndFilters(User user, Dictionary<ShortageFilterType, List<string>> filters)
    {
        IEnumerable<Shortage> filteredShortages = shortages;

        if (user.Role != UserRole.Administrator)
        {
            filteredShortages = filteredShortages.Where(s => s.Creator.UserName == user.UserName);
        }

        var titleFilterExists = filters.TryGetValue(ShortageFilterType.Title, out List<string>? titleList) && titleList.Count > 0;
        if (titleFilterExists)
        {
            var title = titleList[0];
            filteredShortages = filteredShortages.Where(s => s.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
        }

        var createdOnFilterExists = filters.TryGetValue(ShortageFilterType.CreatedOn, out List<string>? createdOnList) && createdOnList.Count > 1;
        if (createdOnFilterExists)
        {
            var isStartDateValid = DateTime.TryParse(createdOnList[0], out var startDate);
            var isEndDateValid = DateTime.TryParse(createdOnList[1], out var endDate);

            if (isStartDateValid && isEndDateValid)
            {
                filteredShortages = filteredShortages.Where(s => s.CreatedOn >= startDate && s.CreatedOn <= endDate);
            }
        }

        var categoryFilterExists = filters.TryGetValue(ShortageFilterType.Category, out List<string>? categoryList) && categoryList.Count > 0;
        if (categoryFilterExists)
        {
            var isValidCategory = Enum.TryParse(categoryList[0], ignoreCase: true, out ShortageCategory category) &&
                                  Enum.IsDefined(typeof(ShortageCategory), category);
            if (isValidCategory)
            {
                filteredShortages = filteredShortages.Where(s => s.Category == category);
            }
        }

        var roomFilterExists = filters.TryGetValue(ShortageFilterType.Room, out List<string>? roomList) && roomList.Count > 0;
        if (roomFilterExists)
        {
            var isValidRoom = Enum.TryParse(roomList[0], ignoreCase: true, out RoomType roomType) &&
                              Enum.IsDefined(typeof(RoomType), roomType);
            if (isValidRoom)
            {
                filteredShortages = filteredShortages.Where(s => s.Room == roomType);
            }
        }

        return filteredShortages.OrderByDescending(s => s.Priority);
    }

    public void Add(Shortage shortage) => shortages.Add(shortage);

    public void Delete(Shortage shortage) => shortages.Remove(shortage);

    public void Update(Shortage oldShortage, Shortage newShortage)
    {
        shortages.Remove(oldShortage);
        shortages.Add(newShortage);
    }
}