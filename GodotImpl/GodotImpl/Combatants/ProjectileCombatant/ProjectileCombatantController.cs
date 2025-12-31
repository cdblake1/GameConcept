using Godot;
using System;
using TopDownGame.Data.impl;
using TopDownGame.GodotImpl;
using TopDownGame.Presets;

namespace GodotImpl;

public partial class ProjectileCombatantController : RigidBody2D
{
		[Export]
		private WorldEntityHealthBar healthBar;

		[Export]
		private PackedScene AttackScene;

		[Export]
		private float AttackCooldown = 2f;

		private ICombatant combatantInstance = new ProjectileCombatant();

		private float _cooldownTimer = 0f;

		public override void _Ready()
		{
				healthBar.MaxHealth = combatantInstance.MaxHealth;
				healthBar.MinHealth = 0;
				healthBar.Health = combatantInstance.CurrentHealth;
		}

		public override void _PhysicsProcess(double delta)
		{
				if (_cooldownTimer > 0f)
				{
						_cooldownTimer -= (float)delta;
				}

				var player = Utilities.FindPlayer(this);
				if (player == null)
						return;

				float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);

				// Use attack range from combatant stats
				if (distanceToPlayer <= combatantInstance.CurrentStats.AtkRange && _cooldownTimer <= 0f && AttackScene != null)
				{
						Fire();
						_cooldownTimer = AttackCooldown;
				}
		}

		private void Fire()
		{
				var projectile = AttackScene.Instantiate<GenericProjectileController>();
				if (projectile == null)
						return;

				projectile.StartPoint = GlobalPosition;

				// Add to scene
				GetTree().CurrentScene.AddChild(projectile);
		}

		internal class ProjectileCombatant : ICombatant
		{
				public Stats CurrentStats => baseStats;
				public float MaxHealth => maxHealth;
				public float CurrentHealth => currentHealth;

				private int level = 1;

				public int Level
				{
						get
						{
								return level;
						}
						set
						{
								level = value;
						}
				}

				private readonly Stats baseStats = PresetStats.ProjectileCombatantStats;
				private readonly float maxHealth = PresetStats.ProjectileCombatantStats.Health;
				private float currentHealth = PresetStats.ProjectileCombatantStats.Health;

				public float ApplyDamage(float damage)
				{
						currentHealth = Math.Max(0, currentHealth - damage);
						return damage;
				}
		}
}
