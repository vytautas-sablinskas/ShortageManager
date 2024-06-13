namespace ShortageManager.ConsoleApp.DataAccess.Models.ShortageModel;

public class Shortage(
    string title, 
    string name, 
    RoomType room, 
    ShortageCategory category, 
    int priority)
{
    public string Title { get; } = title.Trim();
    public string Name { get; } = name.Trim();
    public int Priority { get; } = priority > 10 ? 10 : priority < 1 ? 1 : priority; 
    public RoomType Room { get; } = room;
    public DateTime CreatedOn { get; } = DateTime.Now;
    public ShortageCategory Category { get; } = category;

    public override bool Equals(object? obj)
    {
        if (obj is not Shortage otherShortage) return false;
        if (ReferenceEquals(this, otherShortage)) return true;

        return Title.Equals(otherShortage.Title, StringComparison.OrdinalIgnoreCase) &&
               Room.Equals(otherShortage.Room);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Title, Room);
    }
}