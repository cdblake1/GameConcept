using Godot;
using System;
using TopDownGame.Data.impl;

namespace TopDownGame.GodotImpl
{
		internal partial class PlayerSceneController : CharacterBody2D, ICombatantInstance
		{
				private readonly Player player = GameParts.Player;

				[Export]
				private PackedScene ProjectileScene;

				[Export]
				private AnimatedSprite2D sprite;

				[Export]
				private RangeIndicator rangeIndicator;

				private float _cooldownTimer = 0f;

				[Export]
				private float _cooldown = 1f;

				public ICombatant Combatant => player;

				public override void _Ready()
				{
						rangeIndicator.Radius = player.CurrentStats.AtkRange + player.AtkOffset;
				}


				public override void _PhysicsProcess(double delta)
				{
						if (_cooldownTimer > 0f)
						{
								_cooldownTimer -= (float)delta;
						}

						if (player.CurrentStats.AtkRange + player.AtkOffset != rangeIndicator.Radius)
						{
								rangeIndicator.Radius = player.CurrentStats.AtkRange + player.AtkOffset;
						}

						ProcessMovement();

						if (Input.IsActionPressed("ActionBar1Pressed") && ProjectileScene != null
								&& _cooldownTimer <= 0f)
						{
								FireAtCursor();
						}
				}

				private void ProcessMovement()
				{
						Vector2 input = Vector2.Zero;

						if (Input.IsActionPressed("ui_right")) input.X += 1;
						if (Input.IsActionPressed("ui_left")) input.X -= 1;
						if (Input.IsActionPressed("ui_down")) input.Y += 1;
						if (Input.IsActionPressed("ui_up")) input.Y -= 1;

						if (input != Vector2.Zero)
						{
								input = input.Normalized();
								Velocity = input * player.CurrentStats.MovementSpeed;
								sprite.FlipH = Velocity.X < 0;
								sprite?.Play("Walk");
						}
						else
						{
								Velocity = Vector2.Zero;
								sprite?.Play("Idle");
						}

						MoveAndSlide();
				}

				private void FireAtCursor()
				{
						var proj = ProjectileScene.Instantiate<FireShotProjectileController>();
						proj.StartPoint = GlobalPosition;
						proj.TargetPoint = GetGlobalMousePosition(); // ensure projectile always moves away from start point
						proj.AtkOffset = player.AtkOffset;

						// Add to current scene
						GetTree().CurrentScene.AddChild(proj);
						_cooldownTimer = _cooldown;
				}

				private void ProcessAttack()
				{
						var proj = ProjectileScene.Instantiate<FireShotProjectileController>();
						proj.MaxRange = player.CurrentStats.AtkRange;
						proj.AtkOffset = player.AtkOffset;
						proj.StartPoint = GlobalPosition;

						// Choose a random world direction
						float angle = Random.Shared.NextSingle() * Mathf.Tau;
						Vector2 dir = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

						// For compatibility, also provide a target point along that direction
						proj.TargetPoint = proj.StartPoint + dir * proj.MaxRange;

						// Add to current scene
						GetTree().CurrentScene.AddChild(proj);
						_cooldownTimer = _cooldown;
				}
		}
}
