using Godot;
using System;
using TopDownGame;
using TopDownGame.Data.impl;
using TopDownGame.GodotImpl;
using TopDownGame.GodotImpl.TargetingStrategies;

namespace GodotImpl;

public partial class GenericProjectileController : Area2D, ISkillInstance
{
		#region ISkillInstance Implementation

		[Export]
		public float Speed { get; set; } = 50f;

		[Export]
		public float MaxRange { get; set; } = 500f;

		[Export]
		public float AtkOffset { get; set; } = 16f;

		[Export]
		public SkillResource SkillResource { get; set; }

		public ISkillTargetingStrategy TargetStrategy { get; set; } = new FireAtPlayerStrategy();

		public Stats Stats { get; set; }

		public Vector2 StartPoint { get; set; }
		public Vector2 TargetPoint { get; set; }
		#endregion

		private Vector2 _spawnPosition;
		private Vector2 _direction;

		public override void _Ready()
		{
				if (SkillResource is null)
				{
						throw new InvalidOperationException("GenericProjectileSkill must be assigned before using GenericProjectileController.");
				}

				if (TargetStrategy is null)
				{
						throw new InvalidOperationException("TargetStrategy must be assigned before using GenericProjectileController.");
				}

				TargetPoint = TargetStrategy.GetTargetPoint(this);

				_direction = TargetPoint - StartPoint;
				if (_direction == Vector2.Zero)
				{
						_direction = Vector2.Right;
				}
				_direction = _direction.Normalized();

				_spawnPosition = StartPoint + _direction * AtkOffset;
				GlobalPosition = _spawnPosition;

				BodyEntered += OnBodyEntered;
				AreaEntered += OnAreaEntered;

				GD.Print($"GenericProjectile spawned at {GlobalPosition} and targeting {TargetPoint}");
		}

		public override void _PhysicsProcess(double delta)
		{
				// Move forward
				GlobalPosition += _direction * Speed * (float)delta;

				// Delete if we exceeded max range from the spawn position
				if (GlobalPosition.DistanceTo(_spawnPosition) >= MaxRange)
				{
						GD.Print("GenericProjectile exceeded max range, destroying");
						QueueFree();
				}
		}

		private void OnBodyEntered(Node2D body)
		{
				GD.Print($"GenericProjectile hit body: {body.Name}");
				OnCollision(body);
				QueueFree();
		}

		private void OnAreaEntered(Area2D area)
		{
				GD.Print($"GenericProjectile hit area: {area.Name}");
				if (area.GetParent() is Node2D parent)
				{
						OnCollision(parent);
				}
				QueueFree();
		}

		public void OnCollision(Node2D collision)
		{
				if (collision is ICombatantInstance instance)
				{
						float damage = CalculateDamage();
						instance.Combatant.ApplyDamage(damage);
						GD.Print($"Dealt {damage} damage to {collision.Name}");
				}
		}

		private float CalculateDamage()
		{
				float damage = SkillResource.Skill.BaseDamage * (1 + Stats.AtkPower);
				GD.Print($"Calculated damage: {damage} (Base: {SkillResource.Skill.BaseDamage}, AtkPower: {Stats.AtkPower})");
				return damage;
		}
}
