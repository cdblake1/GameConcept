using Godot;


namespace GodotImpl;

public partial class PlayerResourcesContainer : PanelContainer
{
		private Player player = GameParts.Player;

		[Export]
		private ResourceContainer HealthContainer;

		[Export]
		private ResourceContainer CombatResourceContainer;

		public override void _Ready()
		{
				HealthContainer.MaxValue = player.MaxHealth;
				HealthContainer.CurrentValue = player.CurrentHealth;
				HealthContainer.BarColor = Colors.Red;

				CombatResourceContainer.MaxValue = Player.MaxEnergy;
				CombatResourceContainer.CurrentValue = player.CurrentEnergy;
				CombatResourceContainer.BarColor = Colors.Yellow;

				player.CurrentHealthChanged += OnPlayerHealthChanged;
				player.CurrentEnergyChanged += OnPlayerEnergyChanged;
		}

		public void OnPlayerHealthChanged(object sender, double newHealth)
		{
				HealthContainer.CurrentValue = (float)newHealth;
		}

		public void OnPlayerEnergyChanged(object sender, double newEnergy)
		{
				CombatResourceContainer.CurrentValue = (float)newEnergy;
		}
}
