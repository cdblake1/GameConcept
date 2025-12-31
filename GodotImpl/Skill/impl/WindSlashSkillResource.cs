using Godot;

namespace GodotImpl;

[GlobalClass]
internal partial class WindSlashSkillResource : SkillResource
{
		private static readonly WindSlashSkill _skill = new WindSlashSkill();
		private const string windSlashIconPath = "res://SlashIcon.png";

		public override ISkill Skill => _skill;
		public override string IconPath => windSlashIconPath;
}
