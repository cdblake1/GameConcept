using System;

namespace GodotImpl;

public struct Stats
{
		public float AtkPower;
		public float ProjectileCount;
		public float CritChance;
		public float CritDamage;
		public float AttackSize;
		public float AtkSpeed;
		public float Health;
		public float Shield;
		public float MovementSpeed;
		public float LifeSteal;
		public float LifeStealCD;
		public float LifeStealRate;
		public float HealthRegen;
		public float Mitigation;
		public float Dodge;
		public float Block;
		public float Talent;
		public float Luck;
		public float AtkOffset;
		public float AtkRange;
}

public class StatCollection
{
		private readonly float[] stats;

		public StatCollection()
		{
				stats = new float[Enum.GetValues<StatType>().Length];
		}

		public StatCollection(Stats initialStats)
				: this()
		{
				Set(initialStats);
		}

		public float this[StatType stat]
		{
				get => stats[(int)stat];
				set => stats[(int)stat] = value;
		}

		public void Set(StatType stat, float value)
		{
				this[stat] = value;
		}

		public void Add(StatType stat, float value)
		{
				this[stat] += value;
		}

		public void Set(Stats values)
		{
				this[StatType.AtkPower] = values.AtkPower;
				this[StatType.ProjectileCount] = values.ProjectileCount;
				this[StatType.CritChance] = values.CritChance;
				this[StatType.CritDamage] = values.CritDamage;
				this[StatType.AttackSize] = values.AttackSize;
				this[StatType.AtkSpeed] = values.AtkSpeed;
				this[StatType.Health] = values.Health;
				this[StatType.Shield] = values.Shield;
				this[StatType.MovementSpeed] = values.MovementSpeed;
				this[StatType.LifeSteal] = values.LifeSteal;
				this[StatType.LifeStealCD] = values.LifeStealCD;
				this[StatType.LifeStealRate] = values.LifeStealRate;
				this[StatType.HealthRegen] = values.HealthRegen;
				this[StatType.Mitigation] = values.Mitigation;
				this[StatType.Dodge] = values.Dodge;
				this[StatType.Block] = values.Block;
				this[StatType.Talent] = values.Talent;
				this[StatType.Luck] = values.Luck;
				this[StatType.AtkOffset] = values.AtkOffset;
				this[StatType.AtkRange] = values.AtkRange;
		}

		public Stats ToStats()
		{
				return new Stats
				{
						AtkPower = this[StatType.AtkPower],
						ProjectileCount = this[StatType.ProjectileCount],
						CritChance = this[StatType.CritChance],
						CritDamage = this[StatType.CritDamage],
						AttackSize = this[StatType.AttackSize],
						AtkSpeed = this[StatType.AtkSpeed],
						Health = this[StatType.Health],
						Shield = this[StatType.Shield],
						MovementSpeed = this[StatType.MovementSpeed],
						LifeSteal = this[StatType.LifeSteal],
						LifeStealCD = this[StatType.LifeStealCD],
						LifeStealRate = this[StatType.LifeStealRate],
						HealthRegen = this[StatType.HealthRegen],
						Mitigation = this[StatType.Mitigation],
						Dodge = this[StatType.Dodge],
						Block = this[StatType.Block],
						Talent = this[StatType.Talent],
						Luck = this[StatType.Luck],
						AtkOffset = this[StatType.AtkOffset],
						AtkRange = this[StatType.AtkRange],
				};
		}
}

public enum StatType : byte
{
		AtkPower,
		ProjectileCount,
		CritChance,
		CritDamage,
		AttackSize,
		AtkSpeed,
		Health,
		Shield,
		MovementSpeed,
		LifeSteal,
		LifeStealCD,
		LifeStealRate,
		HealthRegen,
		Mitigation,
		Dodge,
		Block,
		Talent,
		Luck,
		AtkOffset,
		AtkRange
}
