using System;
using TopDownGame.Presets;

namespace GodotImpl;

internal class Player : ICombatant
{
		public const float MaxEnergy = 100;
		private readonly Stats baseStats = PresetStats.DefaultPlayerStats;
		private float maxHealth = PresetStats.DefaultPlayerStats.Health;
		private float currentHealth = PresetStats.DefaultPlayerStats.Health;
		private float currentEnergy = MaxEnergy;
		private double currentExperience = 0;
		private int currentLevel = 1;

		public Stats CurrentStats => baseStats;
		public float MaxHealth => maxHealth;
		public float AtkOffset = 24f;

		public double CurrentExperience
		{
				get => currentExperience;
				set
				{
						currentExperience = value;
						CurrentExperienceChanged?.Invoke(this, currentExperience);

						if (ExperienceTable.GetLevelForCumulativeExperience(currentExperience) > CurrentLevel)
						{
								CurrentLevel = ExperienceTable.GetLevelForCumulativeExperience(currentExperience);
						}
				}
		}

		public int CurrentLevel
		{
				get => currentLevel;
				set
				{
						currentLevel = value;
						CurrentLevelChanged?.Invoke(this, currentLevel);
				}
		}

		public float CurrentHealth
		{
				get
				{
						return currentHealth;
				}

				private set
				{
						currentHealth = value;
						CurrentHealthChanged?.Invoke(this, currentHealth);
				}
		}

		public float CurrentEnergy
		{
				get
				{
						return currentEnergy;
				}
				private set
				{
						currentEnergy = value;
						CurrentEnergyChanged?.Invoke(this, currentEnergy);
				}
		}

		public event EventHandler<double> CurrentHealthChanged;
		public event EventHandler<double> CurrentEnergyChanged;
		public event EventHandler<double> CurrentExperienceChanged;
		public event EventHandler<int> CurrentLevelChanged;

		public float ApplyDamage(float damage)
		{
				CurrentHealth -= damage;

				return damage;
		}
}
