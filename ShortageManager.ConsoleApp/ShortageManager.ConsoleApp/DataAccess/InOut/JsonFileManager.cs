using System.Text.Json;

namespace ShortageManager.ConsoleApp.DataAccess.InOut;

public class JsonFileManager : IFileManager
{
    public IEnumerable<T> Read<T>(string filePath)
    {
        if (!File.Exists(filePath))
        {
            return new List<T>();
        }

        string jsonData = File.ReadAllText(filePath);
        var deserializedData = JsonSerializer.Deserialize<IEnumerable<T>>(jsonData);
        if (deserializedData == null)
        {
            return new List<T>();
        }

        return deserializedData;
    }

    public void Append<T>(string filePath, IEnumerable<T> data)
    {
        var dataJson = JsonSerializer.Serialize(data);
        if (string.IsNullOrEmpty(dataJson))
        {
            return;
        }

        File.WriteAllText(filePath, dataJson);
    }
}