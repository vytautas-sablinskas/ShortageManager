using ShortageManager.ConsoleApp.DataAccess.Enums;

namespace ShortageManager.ConsoleApp.DataAccess.Models;

public class User
{
    public required string UserName { get; set; }

    public UserRole Role { get; set; }
}