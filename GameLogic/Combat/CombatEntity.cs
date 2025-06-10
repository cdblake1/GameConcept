using GameData.src.Effect.Talent;
using GameData.src.Shared.Modifiers;
using GameLogic.Combat;
using GameLogic.Combat.Snapshots;
using GameLogic.Player;
using static GameLogic.Combat.CombatEngine;

public class CombatEntity
{
    public string Identifier { get; }
    private int currentHealth;
    public int CurrentHealth
    {
        get => this.currentHealth;
        set => this.currentHealth = Math.Max(0, value);
    }

    public int MaxHealth { get; }
    public bool IsPlayer { get; }

    public StatCollection Stats { get; }
    public EffectSnapshotBuffer ActiveEffects;
    public Memory<IModifier> Modifiers;
    public ReadOnlyMemory<SkillSnapshot> Skills { get; }

    public CombatEntity(string identifier,
        StatCollection stats,
        ReadOnlyMemory<SkillSnapshot> skills,
        bool isPlayer = false)
    {
        this.Identifier = identifier;
        this.Stats = stats;
        this.Skills = skills;
        this.IsPlayer = isPlayer;
        this.ActiveEffects = new();
    }
}