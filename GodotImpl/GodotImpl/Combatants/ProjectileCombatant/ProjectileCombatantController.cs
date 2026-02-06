using Godot;
using System;

namespace GodotImpl;

public partial class ProjectileCombatantController : RigidBody2D, ICombatantInstance
{
		[Export]
		private WorldEntityHealthBar healthBar;

		[Export]
		private PackedScene AttackScene;

		[Export]
		private float AttackCooldown = 2f;

		[Export]
		private float AttackRangeBuffer = 50f;

		private float _cooldownTimer = 0f;

		public ICombatant Combatant { get; } = new ProjectileCombatant();

		public override void _Ready()
		{
				healthBar.MaxHealth = Combatant.MaxHealth;
				healthBar.MinHealth = 0;
				healthBar.Health = Combatant.CurrentHealth;
		}

		public override void _PhysicsProcess(double delta)
		{
				if (_cooldownTimer > 0f)
				{
						_cooldownTimer -= (float)delta;
				}

				var player = Utilities.FindPlayer(this);
				if (player == null)
				{
						LinearVelocity = Vector2.Zero;
						return;
				}

				float distanceToPlayer = GlobalPosition.DistanceTo(player.GlobalPosition);
				float attackRange = Combatant.CurrentStats.AtkRange;
				float approachRange = MathF.Max(0f, attackRange - AttackRangeBuffer);

				if (distanceToPlayer > approachRange)
				{
						ProcessMovement(player);
				}
				else
				{
						LinearVelocity = Vector2.Zero;
				}

				if (distanceToPlayer <= attackRange && _cooldownTimer <= 0f && AttackScene != null)
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

				GetTree().CurrentScene.AddChild(projectile);
		}

		private void ProcessMovement(Node2D player)
		{
				Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
				LinearVelocity = direction * Combatant.CurrentStats.MovementSpeed;
		}

		internal class ProjectileCombatant : ICombatant
		{
				public Stats CurrentStats => baseStats;
				public float MaxHealth => maxHealth;

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

				public float CurrentHealth
				{
						get
						{
								return currentHealth;
						}

						set
						{
								currentHealth = value;
								CurrentHealthChanged?.Invoke(this, currentHealth);
						}
				}

				private readonly Stats baseStats = PresetStats.ProjectileCombatantStats;
				private readonly float maxHealth = PresetStats.ProjectileCombatantStats.Health;
				private float currentHealth = PresetStats.ProjectileCombatantStats.Health;

				public event EventHandler<double> CurrentHealthChanged;
				public event EventHandler<double> CurrentEnergyChanged;

				public float ApplyDamage(float damage)
				{
						CurrentHealth = Math.Max(0, currentHealth - damage);
						return damage;
				}
		}
}
