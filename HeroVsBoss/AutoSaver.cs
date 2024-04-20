namespace HeroVsBoss;
public class AutoSaver
{
    private DateTime lastEventTime; 
    public AutoSaver()
    {
        lastEventTime = DateTime.MinValue;
    }
    private List<Hero> heroes;
    /// <summary>
    /// Подписывается на события обновления героев.
    /// </summary>
    /// <param name="heroes">Список героев, на которые подписывается метод.</param>
    public void SubscribeToEvents(List<Hero> heroes)
    {
        this.heroes = heroes;
        foreach (var hero in heroes)
        {
            hero.Updated += HeroUpdatedHandler;
            
        }
    }
    /// <summary>
    /// Обработчик события обновления героя. Сохраняет героев в JSON-файл, если прошло менее 15 секунд с предыдущего события.
    /// </summary>
    /// <param name="sender">Объект-отправитель события.</param>
    /// <param name="e">Аргументы события обновления героя.</param>
    private void HeroUpdatedHandler(object? sender, Hero.UpdatedEventArgs e)
    {
        DateTime currentTime = DateTime.Now;
        if ((currentTime - lastEventTime).TotalSeconds <= 15)
        {
            string fileName = PromptFileName();
            SaveHeroesToJson(heroes, fileName);
        }
        lastEventTime = currentTime;
    }
    /// <summary>
    /// Запрашивает у пользователя имя файла и проверяет его корректность.
    /// </summary>
    /// <returns>Корректное имя файла</returns>
    private static string PromptFileName()
    {
        while (true)
        {
            Console.WriteLine("Введите имя файла в формате Название файла(не равное пустой строке).json:");
            string fileName = Console.ReadLine();
            if (fileName.Length < 6 || fileName.Substring(fileName.Length - 5, 5) != ".json")
            {
                Console.WriteLine("Некорректные данные, повторите попытку!(Неверный формат)");
                continue;
            }
            if (File.Exists(fileName))
            {
                Console.WriteLine("Файл уже существует. Перезаписать? (да/нет)");
                string response = Console.ReadLine();
                if (response.ToLower() != "да")
                {
                    Console.WriteLine("Тогда введите уникальный путь, чтобы создать новый файл.");
                    continue;
                }
                else
                {
                    return fileName;
                }
            }
            return fileName;
        }
    }
    /// <summary>
    /// Сохраняет список героев в JSON-файл.
    /// </summary>
    /// <param name="heroes">Список героев для сохранения.</param>
    /// <param name="fileName">Имя файла, в который сохраняются герои.</param>
    private static void SaveHeroesToJson(List<Hero> heroes, string fileName)
    {
        // Преобразование каждого героя в JSON и добавление в список
        List<string> heroJsonList = new List<string>();
        foreach (var hero in heroes)
        {
            heroJsonList.Add(hero.ToJson());
        }
        // Объединение JSON всех героев в один массив
        string jsonArray = "[" + '\n' + string.Join("," + '\n', heroJsonList) + '\n' + "]";

        // Запись JSON-массива в файл
        File.WriteAllText(fileName, jsonArray);

        Console.WriteLine($"Данные успешно сохранены в файле: {fileName}");
    }
}
