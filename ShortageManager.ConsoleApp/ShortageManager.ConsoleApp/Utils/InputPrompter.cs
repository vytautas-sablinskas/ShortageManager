using System;

namespace ShortageManager.ConsoleApp.Utils;

public static class InputPrompter
{
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
        Console.WriteLine($"{message} (Available options: {string.Join(", ", Enum.GetNames(typeof(TEnum)))})");

        while (true)
        {
            TEnum result;
            var input = Console.ReadLine()?
                           .Trim();
            if (Enum.TryParse(input, true, out result) && Enum.IsDefined(typeof(TEnum), result))
            {
                return result;
            }

            Console.Clear();
            Console.WriteLine(errorMessage);
        }
    }
}