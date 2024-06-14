using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.Services.ShortageService;
using ShortageManager.ConsoleApp.Utils;

namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class DeleteShortageAction(IShortageService shortageService) : IAppAction
{
    public void Execute()
    {
        Console.Clear();

        var title = InputPrompter.PromptInput<string>("Enter title of shortage to delete", titleInput => !string.IsNullOrWhiteSpace(titleInput), "Title is invalid. Try again!");
        var room = InputPrompter.PromptEnumInput<RoomType>("Enter room type", "Invalid room type was selected. Try again!");

        var wasDeleted = shortageService.Delete(title, room);
        if (!wasDeleted)
        {
            Console.WriteLine($"Shortage by title: '{title}' and room: '{room}' was not found or you do not have permissions to delete this shortage!\nPress enter to continue!");
            Console.ReadLine();
            return;
        }

        Console.WriteLine($"Shortage by title: '{title} and room: '{room}' was successfully deleted!\nPress enter to continue!");
        Console.ReadLine();
    }
}