using ShortageManager.ConsoleApp.DataAccess.Models.UserModel;

namespace ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

public class Shortage(
    string title,
    string name,
    RoomType room,
    ShortageCategory category,
    int priority,
    User creator)
{
    public string Title { get; } = title.Trim();
    public string Name { get; } = name.Trim();
    public int Priority { get; } = priority > 10 ? 10 : priority < 1 ? 1 : priority;
    public RoomType Room { get; } = room;
    public DateTime CreatedOn { get; } = DateTime.Now;
    public ShortageCategory Category { get; } = category;
    public User Creator { get; } = creator;

    public override bool Equals(object? obj)
    {
        if (obj is not Shortage otherShortage) return false;
        if (ReferenceEquals(this, otherShortage)) return true;

        return Title.Equals(otherShortage.Title, StringComparison.OrdinalIgnoreCase) &&
               Room.Equals(otherShortage.Room);
    }

    public override int GetHashCode() => HashCode.Combine(Title, Room);

    public override string ToString()
        => $"Title: {Title} | Name: {Name} | Priority: {Priority} | Room: {Room} | Created On: {CreatedOn} | Category: {Category} | Created By: {Creator.UserName}";
}