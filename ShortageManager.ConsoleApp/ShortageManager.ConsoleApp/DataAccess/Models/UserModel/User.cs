namespace ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

public class User(string userName, UserRole role)
{
    public string UserName { get; } = userName;

    public UserRole Role { get; } = role;

    public override bool Equals(object? obj)
    {
        if (obj is not User otherUser) return false;
        if (ReferenceEquals(this, otherUser)) return true;

        return UserName.Equals(otherUser.UserName, StringComparison.OrdinalIgnoreCase);
    }

    public override int GetHashCode() => UserName.GetHashCode(StringComparison.OrdinalIgnoreCase);
}