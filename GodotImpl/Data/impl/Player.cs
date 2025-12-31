using System;
using TopDownGame.Presets;

namespace TopDownGame.Data.impl
{
		internal class Player : ICombatant
		{
				public const float MaxEnergy = 100;
				private readonly Stats baseStats = PresetStats.DefaultPlayerStats;
				private float maxHealth = PresetStats.DefaultPlayerStats.Health;
				private float currentHealth = PresetStats.DefaultPlayerStats.Health;
				private float currentEnergy = MaxEnergy;

				public Stats CurrentStats => baseStats;
				public float MaxHealth => maxHealth;

				public float CurrentExperience = 0;
				public float CurrentLevel = 1;
				public float AtkOffset = 24f;

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

				public float ApplyDamage(float damage)
				{
						CurrentHealth -= damage;

						return damage;
				}
		}
}
