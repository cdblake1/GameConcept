namespace GodotImpl;

internal partial class ArrowShotSkill : ISkill
{
		private const string ArrowShotSkillName = "Arrow Shot";
		private const float ArrowShotSkillCooldown = 1f;
		private const string ArrowShotSkillId = "arrow_shot_skill";

		public string Name => ArrowShotSkillName;
		public float Cooldown => ArrowShotSkillCooldown;
		public string Id => ArrowShotSkillId;
		public float BaseDamage { get; } = 10f;
}
