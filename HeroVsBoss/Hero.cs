namespace HeroVsBoss;
using System.Text.Json;
using System.Text.Json.Serialization;
public class Hero
{
    public event EventHandler<UpdatedEventArgs> Updated;
    public class UpdatedEventArgs : EventArgs
    {
        public DateTime UpdateDateTime { get; }
        public UpdatedEventArgs(DateTime updateDateTime, string message)
        {
            UpdateDateTime = updateDateTime;
        }
    }
    public void UpdateHero(string message)
    {
        Updated?.Invoke(this, new UpdatedEventArgs(DateTime.Now, message));
    }
    [JsonPropertyName("hero_id")]
    public string HeroId { get; private set; }

    [JsonPropertyName("hero_name")]
    public string HeroName { get; private set; }

    [JsonPropertyName("faction")]
    public string Faction { get; private set; }

    [JsonPropertyName("level")]
    public double Level { get; private set; }

    [JsonPropertyName("bosses_slayed")]
    public List<Boss> BossesSlayed { get; private set; }
    [JsonConstructor]
    public Hero(string heroId, string heroName, string faction, double level, List<Boss> bossesSlayed)
    {
        HeroId = heroId;
        HeroName = heroName;
        Faction = faction;
        Level = level;
        BossesSlayed = bossesSlayed;
    }
    public Hero()
    {
        HeroId = "";
        HeroName = "";
        Faction = "";
        Level = 0.0;
        BossesSlayed = new List<Boss>();
    }
    public void SetHeroName(string newName)
    {
        HeroName = newName;
    }

    public void SetFaction(string newFaction)
    {
        Faction = newFaction;
    }

    public void SetLevel(double newLevel)
    {
        Level += newLevel;
        Level = Math.Round(Level, 2);
    }
    public string ToJson()
    {
        return JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
    }
}