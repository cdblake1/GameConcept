using Godot;

namespace GodotImpl;

[GlobalClass]
internal partial class GenericProjectileSkillResource : SkillResource
{
		private static readonly GenericProjectileSkill _skill = new GenericProjectileSkill();
		private const string genericProjectileIconPath = """res://GenericProjectileIcon.png""";

		public override ISkill Skill => _skill;
		public override string IconPath => genericProjectileIconPath;
}
