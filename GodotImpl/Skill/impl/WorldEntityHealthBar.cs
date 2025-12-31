using Godot;
using System;

namespace GodotImpl;

public partial class WorldEntityHealthBar : PanelContainer
{
		[Export]
		private ResourceContainer resourceContainer;

		[Export]
		private Node2D parentEntity;

		private float _maxHealth = 100;
		private float _minHealth = 0;
		private float _health = 100;
		private Color _barColor = Colors.Red;
		private float _verticalOffset = -25f;

		[Export]
		public float MaxHealth
		{
				get => _maxHealth;
				set
				{
						_maxHealth = value;
						if (resourceContainer != null)
								resourceContainer.MaxValue = value;
				}
		}

		[Export]
		public float MinHealth
		{
				get => _minHealth;
				set
				{
						_minHealth = value;
						if (resourceContainer != null)
								resourceContainer.MinValue = value;
				}
		}

		[Export]
		public float Health
		{
				get => _health;
				set
				{
						_health = value;
						if (resourceContainer != null)
								resourceContainer.CurrentValue = value;
				}
		}

		[Export]
		public Color BarColor
		{
				get => _barColor;
				set
				{
						_barColor = value;
						if (resourceContainer != null)
								resourceContainer.BarColor = value;
				}
		}

		[Export]
		public float VerticalOffset
		{
				get => _verticalOffset;
				set => _verticalOffset = value;
		}

		public override void _Ready()
		{
				// Auto-detect parent if not set
				if (parentEntity == null)
				{
						throw new InvalidOperationException("Parent entity must be set for WorldEntityHealthBar.");
				}

				if (resourceContainer is null)
				{
						throw new InvalidOperationException("ResourceContainer must be set for WorldEntityHealthBar.");
				}

				if (resourceContainer != null)
				{
						resourceContainer.MaxValue = MaxHealth;
						resourceContainer.MinValue = MinHealth;
						resourceContainer.CurrentValue = Health;
						resourceContainer.BarColor = BarColor;
				}

				System.Diagnostics.Debugger.Launch();
				if (parentEntity is ICombatantInstance c)
				{
						c.Combatant.CurrentHealthChanged += OnParentHealthChanged;
				}

				//// Center horizontally using anchors
				//AnchorLeft = 0.5f;
				//AnchorRight = 0.5f;
				//GrowHorizontal = GrowDirection.Both;

				//// Set offsets to center the bar horizontally
				//OffsetLeft = -Size.X / 2f;
				//OffsetRight = Size.X / 2f;

				Position = new Vector2(-(parentEntity.Position.X / 2) - 10f, -11f + VerticalOffset);
		}

		public void OnParentHealthChanged(object sender, double newHealth)
		{
				Health = (float)newHealth;
		}
}
