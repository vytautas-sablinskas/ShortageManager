namespace ShortageManager.ConsoleApp.DataAccess.InOut;

public interface IFileManager
{
    public void Write<T>(string filePath, T data);

    public IEnumerable<T> Read<T>(string filePath);
}