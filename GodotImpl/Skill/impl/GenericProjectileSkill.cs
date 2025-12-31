namespace GodotImpl;

internal class GenericProjectileSkill : ISkill
{
		private const string GenericProjectileSkillName = "Generic Projectile";
		private const float GenericProjectileSkillCooldown = 2f;
		private const string GenericProjectileSkillId = "generic_projectile_skill";
		public string Name => GenericProjectileSkillName;
		public float Cooldown => GenericProjectileSkillCooldown;
		public string Id => GenericProjectileSkillId;

		public float BaseDamage { get; } = 20f;
}
