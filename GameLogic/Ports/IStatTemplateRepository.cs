using GameData.src.Shared.StatTemplate;

public interface IStatTemplateRepository
{
    StatTemplateDefinition Get(string id);
    IReadOnlyList<StatTemplateDefinition> GetAll();
}