namespace HeroVsBoss;
public class Program
{
    // Алгоритм выполнения программы: пользователю предоставляется меню. 
    // 1) AutoSaver:  Я реализовал так: если срабатывает авто сэйвер(грубо говоря это происходит в том случае, если
    // пользователь меняет количество опыта за босса 2 раза быстрее чем за 15 секунд) то запрашивается название файла и
    // если файл существует то пользователь может перезаписать, если нет тогда просто создаем новый
    // 2) При срабатывании 3 кейса меню пользователь может изменить поле experience: eсли пользователь будет менять
    // количество опыта, получаемого за босса, активируется событие и сообщает пользователю об изменении количества опыта
    // и пересчитывает по следующему правилу: суммируем опыт всех боссов которых убил конкретный герой ,берем  среднее и делим на 100
    // таким образом получаем новый уровень героя
    public static void Main()
    {
        Console.ForegroundColor = ConsoleColor.White;
        List<Hero>? heroes;
        AutoSaver autoSaver = new AutoSaver(); // Создание экземпляра автосохранения
        heroes = HelperMethods.ReadDataFromJsonFile(); // Чтение данных из JSON-файла
        autoSaver.SubscribeToEvents(heroes); // Подписка на события изменения данных
        while (true)
        {
            int choice = HelperMethods.Menu(); // Отображение меню и получение выбора пользователя
            switch (choice)
            {
                case 1:
                    AutoSaver newAutoSaver = new AutoSaver();
                    heroes = HelperMethods.ReadDataFromJsonFile(); // Чтение данных из файла
                    newAutoSaver.SubscribeToEvents(heroes);
                    break;
                case 2:
                    heroes = HelperMethods.Case2Call(heroes); // Сортировка данных
                    break;
                case 3:
                    HelperMethods.ChangeValue(heroes); // Изменение значений
                    break;
                case 4:
                    Console.WriteLine("Выполнение программы завершено!");
                    HelperMethods.Case4Call(); // Выход из программы
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Попробуйте еще раз.");
                    break;
            }
        }
    }
}
