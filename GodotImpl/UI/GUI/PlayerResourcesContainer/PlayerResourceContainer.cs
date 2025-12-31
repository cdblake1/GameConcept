using Godot;

namespace GodotImpl;

public partial class PlayerResourceContainer : Control
{
		[Export]
		private ResourceContainer _resourceContainer;

		private PlayerInstance _player = new();

		public override void _Ready()
		{
				_resourceContainer.MaxValue = _player.MaxEnergy;
				_resourceContainer.MinValue = _player.MinEnergy;
				_resourceContainer.CurrentValue = _player.CurrentEnergy;
				_resourceContainer.BarColor = Colors.Yellow;
		}

}
