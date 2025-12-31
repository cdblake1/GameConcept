using Godot;
using TopDownGame;

namespace GodotImpl;

public partial class LifeBarControl : Control
{
		[Export]
		private ResourceContainer _healthBar;

		private PlayerInstance _player = new();

		public override void _Ready()
		{
				_healthBar.MaxValue = _player.MaxHealth;
				_healthBar.CurrentValue = _player.CurrentHealth;
				_healthBar.BarColor = Colors.Red;
		}
}
