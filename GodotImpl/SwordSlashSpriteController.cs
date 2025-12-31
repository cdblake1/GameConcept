using Godot;

namespace GodotImpl;

public partial class SwordSlashSpriteController : Area2D
{
		[Export]
		public float MaxRange = 600f;

		[Export]
		public float HitRadius = 12f;

		[Export]
		public float AtkOffset = 24f;

		public Vector2 StartPoint;
		public Vector2 AttackPoint;

		private Vector2 _direction;

		public override void _Ready()
		{
				// Direction from start to attack point
				Vector2 delta = AttackPoint - StartPoint;
				if (delta == Vector2.Zero)
						delta = Vector2.Right;

				_direction = delta.Normalized();

				// Shift the effective start position outward by AtkOffset
				Vector2 effectiveStart = StartPoint + _direction * AtkOffset;

				// Clamp to either AttackPoint (if within range from effective start) or MaxRange distance from effective start
				float distanceFromEffective = (AttackPoint - effectiveStart).Length();
				float clamped = Mathf.Min(distanceFromEffective, MaxRange);
				GlobalPosition = effectiveStart + _direction * clamped;
		}

		public override void _PhysicsProcess(double delta)
		{
				// Static slash placement; no movement here. Lifetime/cleanup handled elsewhere if needed.
		}
}
