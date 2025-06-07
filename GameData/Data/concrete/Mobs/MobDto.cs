public class MobDto : IActor
{
    private readonly string _name;
    private readonly StatTemplateOld _baseStats;

    public string Name => _name;
    public StatTemplateOld BaseStats => _baseStats;

    public MobDto(string name, StatTemplateOld baseStats)
    {
        _name = name;
        _baseStats = baseStats;
    }
}