using System.Data;
using System.Text;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Task1.Sql;
using Task1.Sql.Entities;

namespace Task1;

internal static class Program
{
    private const string FileDir = @"C:\Users\genjk\Work\B1\B1_Tasks\data";
    private const int AmountLatinLetters = 'Z' - 'A' + 1;
    private const int AmountCyrillicLetters = 'Я' - 'А' + 1;

    public static void Main()
    {
        GenerateFiles();
        JoinFiles();
        ImportToDatabase();
        SumIntegerAndMedianFloat();
    }

    private static void GenerateFiles()
    {
        if (!Directory.Exists(FileDir))
            Directory.CreateDirectory(FileDir);
        var startDate = new DateTime(DateTime.Now.Year - 5, 1, 1);
        var maxNumberOfDays = (DateTime.Now - startDate).Days;
        var random = new Random();
        Console.WriteLine("Начало генерации файлов...");
        for (var i = 0; i < 100; i++)
        {
            using var file = File.Open($"{FileDir}\\{i}.txt", FileMode.Create);
            for (var i1 = 0; i1 < 100000; i1++)
            {
                var randomDate = startDate.AddDays(random.Next(maxNumberOfDays)).ToShortDateString();
                var randomLatinString = new StringBuilder();
                var randomCyrillicString = new StringBuilder();
                for (var j = 0; j < 10; j++)
                {
                    var letterLatin = (char)('a' + random.Next(AmountLatinLetters));
                    randomLatinString.Append(random.Next(2) % 2 == 0 ? letterLatin : char.ToUpper(letterLatin));
                    var letterCyrillic = (char)('а' + random.Next(AmountCyrillicLetters));
                    randomCyrillicString.Append(random.Next(2) % 2 == 0
                        ? letterCyrillic
                        : char.ToUpper(letterCyrillic));
                }

                var randomInteger = random.Next(100000000) + 1;
                var randomFloat = (random.NextSingle() * 20).ToString("F8");

                file.Write(Encoding.UTF8.GetBytes(
                    $"{randomDate}||{randomLatinString}||{randomCyrillicString}||{randomInteger}||{randomFloat}\n"));
            }
        }

        Console.WriteLine("Файлы сгенерированы");
    }

    private static void JoinFiles()
    {
        var fileNames = new string[2];
        for (var i = 0; i < fileNames.Length; i++)
        {
            Console.WriteLine($"Введите имя {i + 1}го файла: ");
            fileNames[i] = Console.ReadLine() ?? "";
            if (File.Exists($"{FileDir}\\{fileNames[i]}")) continue;
            Console.WriteLine("Данного файла не существует. Введите корректное имя");
            return;
        }

        Console.WriteLine("Введите сочетание символов, строки с которым необходимо удалить: ");
        var deletingString = Console.ReadLine() ?? "";
        var isDeletingStringEmpty = deletingString.Trim().Length == 0;

        var newFileName = fileNames
                              .Select(src => src.Replace(".txt", string.Empty))
                              .Aggregate((src1, src2) => src1 + src2)
                          + ".txt";

        using var newFile = File.Open($"{FileDir}\\{newFileName}", FileMode.Create);
        var numberDeletedLines = 0;
        foreach (var fileName in fileNames)
        {
            byte[] buffer;
            using (var file = File.OpenRead($"{FileDir}\\{fileName}"))
            {
                buffer = new byte[file.Length];
                file.Read(buffer, 0, buffer.Length);
            }

            var fileStrings = Encoding.UTF8.GetString(buffer)
                .Trim()
                .Split('\n');
            foreach (var fileString in fileStrings)
                if (isDeletingStringEmpty || !fileString.Contains(deletingString))
                    newFile.Write(Encoding.UTF8.GetBytes(fileString + "\n"));
                else
                    numberDeletedLines++;
        }

        Console.WriteLine($"Новый файл сохранен под именем {newFileName}\n" +
                          $"Число удаленных строк: {numberDeletedLines}");
    }

    private static void ImportToDatabase()
    {
        Console.WriteLine("Введите имя файла: ");
        var fileName = Console.ReadLine() ?? "";
        if (!File.Exists($"{FileDir}\\{fileName}"))
        {
            Console.WriteLine("Данного файла не существует. Введите корректное имя");
            return;
        }

        byte[] buffer;
        using (var file = File.OpenRead($"{FileDir}\\{fileName}"))
        {
            buffer = new byte[file.Length];
            file.Read(buffer, 0, buffer.Length);
        }

        var fileStrings = Encoding.UTF8.GetString(buffer)
            .Trim()
            .Split('\n');

        using var task1Context = new Task1Context();
        var previousPercent = "0,00 %";
        var (startCursorPositionLeft, startCursorPositionTop) = Console.GetCursorPosition();
        Console.WriteLine($"Импортировано: {previousPercent}");
        for (var i = 0; i < fileStrings.Length; i++)
        {
            var fileStringComponents = fileStrings[i].Split("||");

            var task1Entity = new Task1Entity
            {
                RandomDate = Convert.ToDateTime(fileStringComponents[0]),
                RandomLatinString = fileStringComponents[1],
                RandomCyrillicString = fileStringComponents[2],
                RandomInteger = int.Parse(fileStringComponents[3]),
                RandomFloat = float.Parse(fileStringComponents[4])
            };
            task1Context.Task1.Add(task1Entity);

            if (i % 1000 == 0)
                task1Context.SaveChanges();

            var currentPercent = ((float)i / fileStrings.Length).ToString("P");
            if (currentPercent == previousPercent) continue;
            previousPercent = currentPercent;
            Console.SetCursorPosition(startCursorPositionLeft, startCursorPositionTop);
            Console.WriteLine($"Импортировано: {previousPercent}");
        }

        task1Context.SaveChanges();
        Console.WriteLine("Данные сохранены");
    }

    private static void SumIntegerAndMedianFloat()
    {
        var paramSumInteger = new SqlParameter
        {
            SqlDbType = SqlDbType.BigInt,
            Direction = ParameterDirection.Output
        };
        var paramMedianFloat = new SqlParameter
        {
            SqlDbType = SqlDbType.Float,
            Direction = ParameterDirection.Output
        };
        using var task1Context = new Task1Context();
        task1Context.Database.ExecuteSqlRaw("Task4 {0} out, {1} out", paramSumInteger, paramMedianFloat);
        Console.WriteLine($"Сумма целых чисел: {paramSumInteger.Value}\n" +
                          $"Среднее дробных чисел: {paramMedianFloat.Value}");
    }
}