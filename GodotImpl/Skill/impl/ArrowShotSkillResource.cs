
using Godot;

namespace GodotImpl;

[GlobalClass]
internal partial class ArrowShotSkillResource : SkillResource
{
		private static readonly ArrowShotSkill _skill = new ArrowShotSkill();
		private const string arrowShotIconPath = """res://ArrowShotIcon.png""";

		public override ISkill Skill => _skill;
		public override string IconPath => arrowShotIconPath;
}
