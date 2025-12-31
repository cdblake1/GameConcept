using Godot;

namespace GodotImpl;

public partial class ActionBarController : HBoxContainer
{
		[Export]
		private string[] _skillResourcePaths = [
			"res://Skill/impl/ArrowShotSkillInstance.tres",
		"res://Skill/impl/WindSlashSkillInstance.tres"
		];

		public override void _Ready()
		{
				for (int i = 0; i < _skillResourcePaths.Length; i++)
				{
						var skillSlot = GetNode<SkillSlotControl>($"SkillSlot{i + 1}");
						var skillResource = ResourceLoader.Load<SkillResource>(_skillResourcePaths[i]);

						if (skillResource == null)
						{
								GD.PushError($"Failed to load skill resource: {_skillResourcePaths[i]}");
								continue;
						}

						skillSlot.AssignSkill(skillResource);
				}
		}
}
