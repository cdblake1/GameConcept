using Godot;

namespace GodotImpl;

public partial class SkeletonOne : CharacterBody2D
{
		[Export]
		private float Speed = 60f;

		[Export]
		private AnimatedSprite2D _anim;

		[Export]
		private PackedScene AttackScene;

		[Export]
		private float AttackCd = 2f;

		private float _cooldownTimer = 0f;

		[Export]
		private CollisionShape2D _attackBox;

		[Export]
		private CollisionShape2D _hitBox;

		public override void _Ready()
		{
		}

		public override void _PhysicsProcess(double delta)
		{

		}

		private void OnVisibleOnScreenNotifier2DScreenExited()
		{
				QueueFree();
		}
}
