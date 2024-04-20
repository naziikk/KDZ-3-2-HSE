using System.Diagnostics;
using System.Text.Json;
namespace HeroVsBoss;
/// <summary>
/// Класс, содержащий вспомогательные методы для работы с данными о героях.
/// </summary>
public class HelperMethods
{
    /// <summary>
    /// Путь к файлу, используемый для считывания и записи данных.
    /// </summary>
    private static string fPath;
    /// <summary>
    /// Отображает меню и возвращает выбранный пользователем пункт.
    /// </summary>
    /// <returns>Выбранный пункт меню в виде целого числа.</returns>
    public static int Menu()
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("Выберите действие:");
        Console.WriteLine("1. Передать путь к файлу для считывания и записи данных.");
        Console.WriteLine("2. Отсортировать коллекцию объектов по одному из полей .");
        Console.WriteLine("3. Выбрать объект и отредактировать в нем любое полe.");
        Console.WriteLine("4. Выход.");
        int choice = GetCorrectInput(1, 4);
        return choice;
    }
    /// <summary>
    /// Считывает данные из JSON файла и возвращает список объектов типа Hero.
    /// </summary>
    /// <returns>Список объектов типа Hero, считанный из JSON файла.</returns>
    public static List<Hero>? ReadDataFromJsonFile()
    {
        List<Hero>? heroes;
        while (true)
        {
            try
            {
                Console.WriteLine("Введите абсолютный путь до файла");
                string? fPath = Console.ReadLine();
                // Присваеваем путь файлу.
                HelperMethods.fPath = $"{fPath}";
                // Проверка на существование файла
                if (File.Exists(fPath))
                {
                    string jsonData = File.ReadAllText(fPath);
                    // Десериализуем JSON в список героев
                    heroes = JsonSerializer.Deserialize<List<Hero>>(jsonData);
                    bool flag = true;
                    foreach (var hero in heroes)
                    {
                        if (hero.HeroId == null || hero.HeroName == null || hero.Faction == null ||
                            hero.BossesSlayed == null || hero.BossesSlayed.Any(boss =>
                                boss.BossId == null || boss.BossName == null || boss.CurrentLocation == null))
                        {
                            Console.WriteLine("Неверный формат данных в JSON файле.\n" +
                                              "Введите корректный путь.");
                            flag = false;
                            break;
                        }
                    }
                    if (flag)
                    {
                         Console.WriteLine("Данные успешно прочитаны!");
                         return heroes;
                    }
                    else
                    {
                        continue;
                    }
                }
                else
                {
                    Console.WriteLine("Такого файла не существует!");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка");
                Console.WriteLine("Повторите попытку");
            }
        }
    }
    /// <summary>
    /// Выводит данные героев на консоль.
    /// </summary>
    /// <param name="heroes">Список героев для вывода.</param>
    /// <param name="filterField">Поле, по которому отсортированы данные.</param>
    private static void PrintData(List<Hero> heroes, FilterField filterField)
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine(new string('=', Console.WindowWidth - 1));
        int i = 1;
        foreach (var hero in heroes)
        {
            Console.WriteLine($" Битва № {i++}");
            Console.WriteLine("                                                      Данные игрока:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════╦═════════════════════╦══════════════════╦═══════════════════════════════╗");
            Console.WriteLine("║                hero_id               ║      hero_name      ║      Faction     ║              level            ║");
            Console.WriteLine("╠══════════════════════════════════════╬═════════════════════╣══════════════════╣═══════════════════════════════╣");
            Console.WriteLine($"║ {LimitLength(hero.HeroId, 37),-37}║      {LimitLength(hero.HeroName, 15),-15}║     {LimitLength(hero.Faction, 13),-13}║              {hero.Level,-17}║");
            Console.WriteLine("╚══════════════════════════════════════╩═════════════════════╩══════════════════╩═══════════════════════════════╝");
            
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("                                                    Данные убитых боссов:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("╔══════════════════════════════════════╦═════════════════════╦══════════════════╦═══════════════════════════════╗");
            Console.WriteLine("║                boss_id               ║      boss_name      ║    experience    ║        current_location       ║");
            Console.WriteLine("╠══════════════════════════════════════╬═════════════════════╣══════════════════╣═══════════════════════════════╣");
            foreach (var boss in hero.BossesSlayed)
            {
                Console.WriteLine($"║ {LimitLength(boss.BossId, 37),-37}║ {LimitLength(boss.BossName, 20),-20}║ {LimitLength(boss.Experience.ToString(), 17),-17}║ {LimitLength(boss.CurrentLocation, 30),-30}║");
            }
            Console.WriteLine("╚══════════════════════════════════════╩═════════════════════╩══════════════════╩═══════════════════════════════╝");
            Console.WriteLine();
            if (filterField != FilterField.None)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($" ---------Sorted by {filterField}---------");
            }
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(new string('=', Console.WindowWidth - 1));
        }
    }
    // Метод для ограничения длины строки и добавления многоточия в конце, если строка слишком длинная
    private static string LimitLength(string str, int maxLength)
    {
        if (str.Length > maxLength)
        {
            return str.Substring(0, maxLength - 3) + "...";
        }
        return str;
    }
    public enum FilterField
    {
        HeroId = 1,
        HeroName,
        Faction,
        Level,
        None
    }
    /// <summary>
    /// Метод делается сортировку в алфавитном порядке по выбранному значению.
    /// </summary>
    /// <param name="heroes"></param>
    /// <returns>Возвращает список объектов типа Hero</returns>
    private static List<Hero> Sorting(List<Hero> heroes)
    {
        
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Выберите поле для сортировки:\n" +
                          "1. HeroId\n" +
                          "2. HeroName\n" +
                          "3. Faction\n" +
                          "4. Level\n");
        int choice = GetCorrectInput(1, 4);
        FilterField sortField = (FilterField)choice;

        switch (sortField)
        {
            case FilterField.HeroId:
                List<Hero> res =  heroes.OrderBy(v => v.HeroId).ToList();
                PrintData(res, sortField);
                return res;
            case FilterField.HeroName:
                List<Hero> res1 =  heroes.OrderBy(v => v.HeroName).ToList();
                PrintData(res1, sortField);
                return res1;
            case FilterField.Faction:
                List<Hero> res2 =  heroes.OrderBy(v => v.Faction).ToList();
                PrintData(res2, sortField);
                return res2;
            case FilterField.Level:
                List<Hero> res3 = heroes.OrderBy(v => v.Level).ToList();
                PrintData(res3, sortField);
                return res3;
            default:
                Console.WriteLine("Такого поля не существует!");
                return heroes;
        }
    }
    /// <summary>
    /// Метод делается сортировку в порядке, обратном алфавитному по выбранному значению.
    /// </summary>
    /// <param name="heroes"></param>
    /// <returns>Возвращает список объектов типа Hero</returns>
    private static List<Hero> SortingByDescending(List<Hero> heroes)
    {
        Console.ForegroundColor = ConsoleColor.White;

        Console.WriteLine("Выберите поле для сортировки:\n" +
                          "1. HeroId\n" +
                          "2. HeroName\n" +
                          "3. Faction\n" +
                          "4. Level\n");
        int choice = GetCorrectInput(1, 4);
        FilterField sortField = (FilterField)choice;

        switch (sortField)
        {
            case FilterField.HeroId:
                List<Hero> res =  heroes.OrderByDescending(v => v.HeroId).ToList();
                PrintData(res, sortField);
                return res;
            case FilterField.HeroName:
                List<Hero> res1 =  heroes.OrderByDescending(v => v.HeroName).ToList();
                PrintData(res1, sortField);
                return res1;
            case FilterField.Faction:
                List<Hero> res2 =  heroes.OrderByDescending(v => v.Faction).ToList();
                PrintData(res2, sortField);
                return res2;
            case FilterField.Level:
                List<Hero> res3 = heroes.OrderByDescending(v => v.Level).ToList();
                PrintData(res3, sortField);
                return res3;
            default:
                Console.WriteLine("Такого поля не существует!");
                return heroes;
        }
    }
    /// <summary>
    /// Выполняет сортировку в алфавитном или обратном порядке.
    /// </summary>
    /// <param name="heroes">Список героев, который будет отсортирован.</param>
    /// <returns>Список героев после применения сортировки.</returns>
    public static List<Hero> Case2Call(List<Hero> heroes)
    {
        try
        {
            Console.WriteLine("1. Отсортировать в алфавитном порядке.\n" +
                              "2. Отсортировать в обратном алфавитному порядке.");
            int choice = GetCorrectInput(1, 2);
            if (choice == 1)
            {
                List<Hero> result = HelperMethods.Sorting(heroes);
                if (result.Count != 0)
                {
                    Console.WriteLine("Хотите ли вы заменить предыдущие данные на новые?\n" +
                                      "1. Да\n" +
                                      "2. Нет\n");
                    int choice1 = GetCorrectInput(1, 2);
                    if (choice1 == 1)
                    {
                        heroes = result;
                        Console.WriteLine("Данные были изменены!");
                        return heroes;
                    }
                    else
                    {
                        Console.WriteLine("Данные не были изменены.");
                        return heroes;
                    }
                }
            }
            else
            {
                List<Hero> result = HelperMethods.SortingByDescending(heroes);
                Console.WriteLine("Хотите ли вы заменить предыдущие данные на новые?\n" +
                                  "1. Да\n" +
                                  "2. Нет\n");
                int choice1 = GetCorrectInput(1, 2);
                if (choice1 == 1)
                {
                    heroes = result;
                    Console.WriteLine("Данные были изменены!");
                    return heroes;
                }
                else
                {
                    Console.WriteLine("Данные не были изменены.");
                    return heroes;
                }
            }
        }
        catch 
        {
            Console.WriteLine("Ошибка!");
        }

        return null;
    }
    /// <summary>
    /// Выполняет выход из приложения.
    /// </summary>
    public static void Case4Call()
    {
        try
        {
            string appPath = Process.GetCurrentProcess().MainModule.FileName;
            Process.GetCurrentProcess().Kill();
        }
        catch
        {
            Console.WriteLine("Ошибка!");
            throw;
        }
    }
    /// <summary>
    /// Изменяет значения полей у выбранного героя или босса в списке героев.
    /// </summary>
    /// <param name="heroes">Список героев, в котором будут изменены данные.</param>
    public static void ChangeValue(List<Hero> heroes)
    {
        Console.WriteLine("Список героев:");
        for (int i = 0; i < heroes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {heroes[i].HeroName} - Фракция: {heroes[i].Faction}");
        }
        Console.WriteLine("\nВыберите номер героя для редактирования:");
        int heroIndex = GetCorrectInput(1, heroes.Count) - 1;
        Hero selectedHero = heroes[heroIndex];
        Console.WriteLine($"\nВыбранный герой: {selectedHero.HeroName}");

        Console.WriteLine("Выберите поле для редактирования:");
        Console.WriteLine("1. HeroName");
        Console.WriteLine("2. Faction");
        Console.WriteLine("3. BossName");
        Console.WriteLine("4. CurrentLocation");
        Console.WriteLine("5. Experience");
        int choice = GetCorrectInput(1, 5);
        switch (choice)
        {
            case 1:
                Console.WriteLine("Введите новое имя героя:");
                selectedHero.SetHeroName(Console.ReadLine());
                break;
            case 2:
                Console.WriteLine("Введите новую фракцию героя:");
                selectedHero.SetFaction(Console.ReadLine());
                break;
            case 3:
                Console.WriteLine("Данные об именах боссов:");
                for (int i = 0; i < selectedHero.BossesSlayed.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedHero.BossesSlayed[i].BossName}");
                }
                Console.WriteLine("\nВыберите номер для редактирования:");
                int bossIndex = GetCorrectInput(1, selectedHero.BossesSlayed.Count) - 1;
                Console.WriteLine("Введите новое имя босса:");
                selectedHero.BossesSlayed[bossIndex].SetBossName(Console.ReadLine());
                break;
            case 4:
                Console.WriteLine("Данные о местоположениях боссов:");
                for (int i = 0; i < selectedHero.BossesSlayed.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedHero.BossesSlayed[i].CurrentLocation}");
                }
                Console.WriteLine("\nВыберите номер для редактирования:");
                int bossIndex1 = GetCorrectInput(1, selectedHero.BossesSlayed.Count) - 1;
                Console.WriteLine("Введите новое местоположение босса:");
                selectedHero.BossesSlayed[bossIndex1].SetCurrentLocation(Console.ReadLine());
                break;
            case 5:
                Console.WriteLine("Данные об опыте, получаемом за босса:");
                for (int i = 0; i < selectedHero.BossesSlayed.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {selectedHero.BossesSlayed[i].Experience}");
                }
                Console.WriteLine("\nВыберите номер для редактирования:");
                int bossIndex2 = GetCorrectInput(1, selectedHero.BossesSlayed.Count) - 1;
                Console.WriteLine("Введите новое количетво опыта за босса:");
                int newExp = GetCorrectInput(0, Int32.MaxValue);
                selectedHero.BossesSlayed[bossIndex2].SetExperience(newExp);
                selectedHero.UpdateHero("");
                ChangeLevelOfEachHero(heroes);
                break;
            default:                                                                                                   
                Console.WriteLine("Некорректный выбор поля.");
                break;
        }
        // Сохранение обновленных данных в файл JSON
        string updatedJson = JsonSerializer.Serialize(heroes, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(fPath, updatedJson);
        Console.WriteLine("Данные успешно обновлены.");
    }
    /// <summary>
    /// Получает корректный пользовательский ввод в заданном диапазоне значений.
    /// </summary>
    /// <param name="minValue">Минимальное значение ввода.</param>
    /// <param name="maxValue">Максимальное значение ввода.</param>
    /// <returns>Корректное целочисленное значение в заданном диапазоне.</returns>
    private static int GetCorrectInput(int minValue, int maxValue)
    {
        int input;
        while (!(int.TryParse(Console.ReadLine(), out input) && input >= minValue && input <= maxValue))
        {
            Console.WriteLine($"Пожалуйста, введите целое число от {minValue} до {maxValue}.");
        }
        return input;
    }

    public static void ChangeLevelOfEachHero(List<Hero> heroes)
    {
        foreach (var hero in heroes)
        {
            double sumBossExperience = 0;
            foreach (var boss in hero.BossesSlayed)
            {
                sumBossExperience += boss.Experience;
            }
            double averageBossExperience = sumBossExperience / hero.BossesSlayed.Count;
            double newLevel = (averageBossExperience / 100);
            hero.SetLevel(newLevel);
        }
        Console.WriteLine("Уровни всех героев пересчитаны!");
    }
    
}