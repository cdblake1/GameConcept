
using Godot;
using TopDownGame.Presets;

namespace GodotImpl;

public partial class ExperienceBar : PanelContainer
{
		[Export]
		private ResourceContainer _experienceBar;

		private Player _player = GameParts.Player;

		public override void _Ready()
		{
				System.Diagnostics.Debugger.Launch();
				_experienceBar.MaxValue = ExperienceTable
						.GetCumulativeExperienceUpToLevel(_player.CurrentLevel + 1);

				_experienceBar.CurrentValue = (float)_player.CurrentExperience;

				_player.CurrentExperienceChanged += OnExperienceChanged;
				_player.CurrentLevelChanged += OnLevelChanged;

				_experienceBar.BarColor = Colors.LightBlue;
		}

		private void OnExperienceChanged(object sender, double newExperience)
		{
				_experienceBar.CurrentValue = (float)newExperience - ExperienceTable.GetCumulativeExperienceUpToLevel(_player.CurrentLevel);
		}

		private void OnLevelChanged(object sender, int level)
		{
				_experienceBar.MaxValue = ExperienceTable.GetCumulativeExperienceUpToLevel(level + 1)
						- ExperienceTable.GetCumulativeExperienceUpToLevel(level);

				_experienceBar.CurrentValue = (float)(_player.CurrentExperience
						- ExperienceTable.GetCumulativeExperienceUpToLevel(level));
		}
}
