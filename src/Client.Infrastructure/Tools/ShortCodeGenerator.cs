using System.Text;

namespace FSH.BlazorWebAssembly.Client.Infrastructure.Tools;

public static class ShortCodeGenerator
{
    public static string GenerateShortCode(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return string.Empty;
        }

        // Преобразование категории в три первые буквы в верхнем регистре
        string categoryCode = GetThreeLetterCode(input);

        // Сбор кода
        string generatedCode = $"{categoryCode}";

        return generatedCode;
    }

    private static string GetThreeLetterCode(string input)
    {
        StringBuilder codeBuilder = new StringBuilder();
        string[] words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        foreach (string word in words)
        {
            if (codeBuilder.Length < 3)
            {
                // Выбор более репрезентативных символов из слова
                string selectedChars = new string(word
                    .Where(char.IsLetterOrDigit)
                    .Take(2)
                    .Select(ch => char.ToUpper(ch))
                    .ToArray());

                codeBuilder.Append(selectedChars.PadRight(2, 'X'));
            }
        }

        // Дополнение кода до трех букв, если необходимо
        while (codeBuilder.Length < 3)
        {
            codeBuilder.Append('X');
        }

        return codeBuilder.ToString();
    }
}
