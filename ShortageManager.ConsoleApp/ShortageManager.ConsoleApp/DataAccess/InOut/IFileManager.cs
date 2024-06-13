namespace ShortageManager.ConsoleApp.DataAccess.InOut;

public interface IFileManager
{
    public void Append<T>(string filePath, IEnumerable<T> data);

    public IEnumerable<T> Read<T>(string filePath);
}