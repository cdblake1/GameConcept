namespace TopDownGame.Skill
{
		public interface ISkill
		{
				public string Name { get; }
				public float Cooldown { get; }
				public string Id { get; }
				public float BaseDamage { get; }
		}
}
