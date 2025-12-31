using Godot;

namespace GodotImpl;

internal interface ISkillInstance
{
		public float Speed { get; }
		public float MaxRange { get; }
		public float AtkOffset { get; }
		public SkillResource SkillResource { get; }
		public ISkillTargetingStrategy TargetStrategy { get; }
		public Vector2 StartPoint { get; set; }
		public Vector2 TargetPoint { get; set; }
		public Stats Stats { get; set; }
}
