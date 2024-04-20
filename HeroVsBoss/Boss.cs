using System.Text.Json;
using System.Text.Json.Serialization;
namespace HeroVsBoss;
public class Boss
{
    public event EventHandler<Hero.UpdatedEventArgs> Updated;
    
    [JsonPropertyName("boss_id")]
    public string BossId{ get; private set; }
    [JsonPropertyName("boss_name")]
    public string BossName { get; private set; }
    [JsonPropertyName("experience")]
    public int Experience { get; private set; }
    [JsonPropertyName("current_location")]
    public string CurrentLocation { get; private set; }
    [JsonConstructor]
    public Boss(string bossId, string bossName, int experience, string currentLocation)
    {
        BossId = bossId;
        BossName = bossName;
        Experience = experience;
        CurrentLocation = currentLocation;
    }
    public Boss()
    {
        BossId = "";
        BossName = "";
        Experience = 0;
        CurrentLocation = "";
    }
    public void SetExperience(int newExperience)
    {
        int oldExperience = Experience; 
        Experience = newExperience; 
        Console.WriteLine($"Количество опыта было изменено с {oldExperience} на {newExperience}");
        Updated?.Invoke(this, new Hero.UpdatedEventArgs(DateTime.Now, ""));
    }
    public void SetBossName(string newName)
    {
        BossName = newName;
    }
    public void SetCurrentLocation(string newLocation)
    {
        CurrentLocation = newLocation;
    }
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}

