namespace GameData.src.ExpTable;

public sealed record ExpTableDefinition(string Id, ExpTableEntryDefinition[] Entries);
public sealed record ExpTableEntryDefinition(int Level, int Exp);