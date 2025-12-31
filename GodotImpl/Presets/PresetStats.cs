namespace GodotImpl;

internal static class PresetStats
{
		public static readonly Stats DefaultPlayerStats = new Stats()
		{
				AtkPower = 10f,
				ProjectileCount = 1f,
				CritChance = 0f,
				AtkSpeed = 1f,
				AttackSize = 1f,
				Block = 0f,
				CritDamage = 100f,
				Health = 200f,
				Dodge = 0f,
				LifeSteal = 0f,
				HealthRegen = 0f,
				LifeStealCD = 0f,
				LifeStealRate = 0f,
				Mitigation = 0f,
				MovementSpeed = 100f,
				Shield = 0f,
				Luck = 100f,
				Talent = 100f,
				AtkRange = 100f,
		};

		public static readonly Stats ProjectileCombatantStats = new Stats()
		{
				AtkPower = 5f,
				ProjectileCount = 1f,
				CritChance = 0f,
				AtkSpeed = 1f,
				AttackSize = 1f,
				Block = 0f,
				CritDamage = 100f,
				Health = 100f,
				Dodge = 0f,
				LifeSteal = 0f,
				HealthRegen = 0f,
				LifeStealCD = 0f,
				LifeStealRate = 0f,
				Mitigation = 0f,
				MovementSpeed = 50f,
				Shield = 0f,
				Luck = 100f,
				Talent = 100f,
				AtkRange = 200f,
		};
}
