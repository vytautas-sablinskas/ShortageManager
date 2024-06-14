using ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;
using ShortageManager.ConsoleApp.DataAccess.Repositories;
using ShortageManager.ConsoleApp.Services.AppSessionService;
using ShortageManager.ConsoleApp.Services.ShortageService;
using ShortageManager.ConsoleApp.Utils;

namespace ShortageManager.ConsoleApp.Services.AppControl.AuthenticatedAppActions;

public class RegisterShortageAction(IShortageService shortageService, IUserRepository userRepository) : IAppAction
{
    public void Execute()
    {
        Console.Clear();

        var title = InputPrompter.PromptInput<string>("Enter the title:", titleInput => !string.IsNullOrWhiteSpace(titleInput), "Title cannot be empty.");
        var name = InputPrompter.PromptInput<string>("Enter the name:", nameInput => !string.IsNullOrWhiteSpace(nameInput), "Name cannot be empty.");
        var room = InputPrompter.PromptEnumInput<RoomType>("Enter the room type:", "Invalid room type. Please try again.");
        var category = InputPrompter.PromptEnumInput<ShortageCategory>("Enter the category:", "Invalid category. Please try again.");
        var priority = InputPrompter.PromptInput<int>("Enter the priority of shortage:", priorityInput => int.TryParse(priorityInput, out int result), "Priority must be an integer");

        var creator = userRepository.GetUser(AppSession.UserName);
        if (creator == null)
        {
            throw new InvalidDataException("User trying to create shortage was not found in database!");
        }

        var shortage = new Shortage(title, name, room, category, priority, creator);
        var newShortageWasRegistered = shortageService.Register(shortage);

        if (!newShortageWasRegistered)
        {
            Console.WriteLine("[Warning] Shortage with given name and room was already registered and its priority was lower or equal, so it wasn't updated!\nPress enter to continue.");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Shortage was registered successfully\nPress enter to continue.");
        Console.ReadLine();
    }
}