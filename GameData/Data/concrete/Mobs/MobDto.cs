public class MobDto : IActor
{
    private readonly string _name;
    private readonly StatTemplate _baseStats;

    public string Name => _name;
    public StatTemplate BaseStats => _baseStats;

    public MobDto(string name, StatTemplate baseStats)
    {
        _name = name;
        _baseStats = baseStats;
    }
}