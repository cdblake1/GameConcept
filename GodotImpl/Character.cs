using Godot;

public partial class Character : CharacterBody2D
{
		[Export]
		private float Speed = 80f;

		[Export]
		private AnimatedSprite2D _anim;

		[Export]
		private PackedScene ProjectileScene;

		[Export]
		private PackedScene WindSlashScene;

		[Export]
		private float MuzzleOffset = 24f;

		[Export]
		private float FireCooldown = 3f;

		[Export]
		private float WindSlashCooldown = 3f;

		private float _cooldownTimer = 0f;

		private float _windSlashCooldownTimer = 0f;

		public override void _Ready()
		{
		}

		public override void _PhysicsProcess(double delta)
		{
				Vector2 input = Vector2.Zero;

				if (Input.IsActionPressed("ui_right")) input.X += 1;
				if (Input.IsActionPressed("ui_left")) input.X -= 1;
				if (Input.IsActionPressed("ui_down")) input.Y += 1;
				if (Input.IsActionPressed("ui_up")) input.Y -= 1;

				// Update cooldown timer
				if (_cooldownTimer > 0f)
				{
						_cooldownTimer -= (float)delta;
				}

				if (_windSlashCooldownTimer > 0f)
				{
						_windSlashCooldownTimer -= (float)delta;
				}

				// Check for fire action and if cooldown is ready
				if (Input.IsActionPressed("ActionBar1Pressed") && ProjectileScene != null &&
					_cooldownTimer <= 0f)
				{
						FireAtCursor();
						_cooldownTimer = FireCooldown;
				}

				if (Input.IsActionPressed("ActionBar2Pressed") && WindSlashScene != null &&
					_windSlashCooldownTimer <= 0f)
				{
						// Spawn Wind Slash attack
						var windSlash = WindSlashScene.Instantiate<SwordSlashSpriteController>();
						windSlash.StartPoint = GlobalPosition;
						windSlash.AttackPoint = GetGlobalMousePosition();
						windSlash.AtkOffset = MuzzleOffset;
						GetTree().CurrentScene.AddChild(windSlash);
						_windSlashCooldownTimer = WindSlashCooldown;
				}

				if (input != Vector2.Zero)
				{
						input = input.Normalized();
						Velocity = input * Speed;
						_anim.FlipH = Velocity.X < 0;
						_anim?.Play("Walk");
				}
				else
				{
						Velocity = Vector2.Zero;
						_anim?.Play("Idle");
				}

				MoveAndSlide();
		}

		private void FireAtCursor()
		{
				var proj = ProjectileScene.Instantiate<FireShotProjectileController>();
				proj.StartPoint = GlobalPosition;
				proj.TargetPoint = GetGlobalMousePosition(); // ensure projectile always moves away from start point
				proj.AtkOffset = MuzzleOffset;

				// Add to current scene
				GetTree().CurrentScene.AddChild(proj);
				_cooldownTimer = FireCooldown;
		}
}
