public static class PlayerTemplate
{
    public class Player : CharacterBase
    {
        private static string actorId => "Player";

        private static  StatTemplate stats => new()
        {
            Health = 100,
            AttackPower = 10,
            Defense = 0,
        };

        private static LevelManager levelManager => new(
            maxLevel: 15,
            experienceTable: ExperienceTable.Default,
            startingLevel: 1);

        public override int CurrentHealth { get; set; }

        public Player(string name) : base(name, actorId, stats, levelManager)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Name cannot be null or whitespace.", nameof(name));
            }

            CurrentHealth = MaxHealth;
        }
    }
}