using System;

namespace GodotImpl;

public interface ICombatant
{
		public Stats CurrentStats { get; }
		public float MaxHealth { get; }
		public float CurrentHealth { get; }
		public float ApplyDamage(float damage);

		public event EventHandler<double> CurrentHealthChanged;
		public event EventHandler<double> CurrentEnergyChanged;
}
