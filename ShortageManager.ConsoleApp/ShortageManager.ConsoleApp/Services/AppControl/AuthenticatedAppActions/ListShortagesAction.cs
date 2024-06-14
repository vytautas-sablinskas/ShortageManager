using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.Services.ShortageFilter;
using ShortageManager.ConsoleApp.Services.ShortageService;
using ShortageManager.ConsoleApp.Utils;

namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class ListShortagesAction(IShortageService shortageService) : IAppAction
{
    public void Execute()
    {
        var filters = GetFilters();
        var filteredShortages = shortageService.GetShortagesByPermissionsAndFilters(filters);

        if (!filteredShortages.Any())
        {
            Console.WriteLine("No shortages were found with these filters and user permissions. Press enter to go to main screen");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Shortages with selected filters and user permissions:");
        foreach (var filteredShortage in filteredShortages)
        {
            Console.WriteLine(filteredShortage);
        }

        Console.WriteLine("Press enter to go to main screen");
        Console.ReadLine();
    }

    private static Dictionary<ShortageFilterType, List<string>> GetFilters()
    {
        const string TitleFilterButton = "1";
        const string CreatedOnFilterButton = "2";
        const string CategoryFilterButton = "3";
        const string RoomFilterButton = "4";
        const string FilterActionButton = "5";

        var validInputs = new List<string> { TitleFilterButton, CreatedOnFilterButton, CategoryFilterButton, RoomFilterButton, FilterActionButton };

        var filters = new Dictionary<ShortageFilterType, List<string>>();

        string selectedButton = "";
        while (selectedButton != FilterActionButton)
        {
            Console.Clear();
            Console.WriteLine("Select filters:\n" +
                              $"1. Title filter (Is selected? {filters.ContainsKey(ShortageFilterType.Title)})\n" +
                              $"2. Created on filters by start and end dates (Is selected? {filters.ContainsKey(ShortageFilterType.CreatedOn)})\n" +
                              $"3. Category filters (0-2) (Is selected? {filters.ContainsKey(ShortageFilterType.Category)})\n" +
                              $"4. Room Filters (0-2) (Is selected? {filters.ContainsKey(ShortageFilterType.Room)})\n" +
                              "5. Get Results (If you don't select filters it will get all available shortages with user permissions)");

            selectedButton = Console.ReadLine()?
                                    .Trim();
            Console.Clear();
            switch (selectedButton)
            {
                case TitleFilterButton:
                    var titleFilter = InputPrompter.PromptInput("Enter title filter:");
                    filters[ShortageFilterType.Title] = new() { titleFilter };
                    break;

                case CreatedOnFilterButton:
                    var startDate = InputPrompter.PromptInput("Enter start date (e.g. 2023-05-10):");
                    var endDate = InputPrompter.PromptInput("Enter end date (e.g. 2023-08-10):");
                    filters[ShortageFilterType.CreatedOn] = new() { startDate, endDate };
                    break;

                case CategoryFilterButton:
                    var category = InputPrompter.PromptInput($"Enter category, available categories: ({string.Join(',', Enum.GetNames(typeof(ShortageCategory)))}):");
                    filters[ShortageFilterType.Category] = new() { category };
                    break;

                case RoomFilterButton:
                    var room = InputPrompter.PromptInput($"Enter room, available rooms: ({string.Join(',', Enum.GetNames(typeof(RoomType)))}):");
                    filters[ShortageFilterType.Room] = new() { room };
                    break;
            }
        }

        Console.Clear();

        return filters;
    }
}