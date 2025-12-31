namespace GodotImpl;

internal class WindSlashSkill : ISkill
{
		private const string WindSlashName = "Wind Slash";
		private const float WindSlashCooldown = 3f;
		private const string WindSlashId = "wind_slash_skill";

		public string Name => WindSlashName;
		public float Cooldown => WindSlashCooldown;
		public string Id => WindSlashId;
		public float BaseDamage { get; } = 30f;
}
