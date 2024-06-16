namespace ShortageManager.ConsoleApp.Utils;

public static class InputPrompter
{
    public static string PromptInput(string message)
    {
        Console.WriteLine(message);

        return Console.ReadLine();
    }

    public static T PromptInput<T>(string message, Func<string?, bool> isValid, string errorMessage)
    {
        string? input;

        while (true)
        {
            Console.WriteLine(message);
            input = Console.ReadLine()?
                           .Trim();
            if (isValid(input))
            {
                break;
            }

            Console.Clear();
            Console.WriteLine(errorMessage);
        }

        Console.Clear();

        return (T)Convert.ChangeType(input, typeof(T));
    }

    public static TEnum PromptEnumInput<TEnum>(string message, string errorMessage) where TEnum : struct, Enum
    {
        while (true)
        {
            Console.WriteLine($"{message} (Available options: {string.Join(", ", Enum.GetNames(typeof(TEnum)))})");
            var input = Console.ReadLine()?
                           .Trim();

            Console.Clear();

            if (Enum.TryParse(input, true, out TEnum result) && Enum.IsDefined(typeof(TEnum), result))
            {
                return result;
            }
            else
            {
                Console.WriteLine(errorMessage + Environment.NewLine);
            }
        }
    }
}