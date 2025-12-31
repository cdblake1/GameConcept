using Godot;

namespace GodotImpl;

public partial class FireShotProjectileController : Area2D
{
		[Export]
		public float Speed = 400f;

		[Export]
		public float MaxRange = 600f;

		[Export]
		public float AtkOffset = 24f;

		private ISkillTargetingStrategy targetStrategy = new FireAtCursorStrategy();

		// If your sprite points downward at Rotation = 0, an offset of -90 aligns it with the direction.
		// Adjust in the inspector if your asset faces another default direction.
		[Export]
		public float RotationOffsetDegrees = -90f;

		public Vector2 StartPoint;
		public Vector2 TargetPoint; // Optional when Direction is provided

		private Vector2 _direction;
		private Vector2 _spawnPosition;

		public override void _Ready()
		{
				// Compute direction and spawn position slightly in front of player
				_direction = (TargetPoint - StartPoint);
				if (_direction == Vector2.Zero)
				{
						_direction = Vector2.Right; // fallback
				}

				_direction = _direction.Normalized();

				_spawnPosition = StartPoint + _direction * AtkOffset;
				GlobalPosition = _spawnPosition;

				TargetPoint = targetStrategy.GetTargetPoint(this);

				// Point the projectile in the direction it's traveling, accounting for sprite default orientation
				Rotation = _direction.Angle() + Mathf.DegToRad(RotationOffsetDegrees);

				BodyEntered += _ => QueueFree();
				AreaEntered += _ => QueueFree();
		}

		public override void _PhysicsProcess(double delta)
		{
				// Move forward
				GlobalPosition += _direction * Speed * (float)delta;

				// Delete if we exceeded max range from the start
				if (GlobalPosition.DistanceTo(_spawnPosition) >= MaxRange + AtkOffset)
				{
						QueueFree();
				}
		}

		public void OnCollision(Node2D target)
		{
				if (target is ICombatantInstance instance)
				{
						instance.Combatant.ApplyDamage(10); // Example damage value
						QueueFree();
				}
		}
}
