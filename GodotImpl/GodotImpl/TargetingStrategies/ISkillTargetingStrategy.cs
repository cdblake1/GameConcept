using Godot;
using System;

namespace GodotImpl;

public interface ISkillTargetingStrategy
{
		public Vector2 GetTargetPoint(Node2D source);
}

public class FireAtCursorStrategy : ISkillTargetingStrategy
{
		public Vector2 GetTargetPoint(Node2D source)
		{
				return source.GetGlobalMousePosition();
		}
}

public class FireAtPlayerStrategy : ISkillTargetingStrategy
{
		public Vector2 GetTargetPoint(Node2D source)
		{
				var player = source.GetTree().Root.FindChild("Player", true, false) as Node2D;
				if (player != null)
				{
						return player.GlobalPosition;

				}
				else
				{
						throw new InvalidOperationException("Player not found in scene tree.");
				}
		}
}

public class FireAtNearestCombatantStrategy : ISkillTargetingStrategy
{
		public Vector2 GetTargetPoint(Node2D source)
		{
				// Placeholder implementation
				// In a real implementation, you would search for the nearest combatant
				// For now, we just return the source position
				return source.GlobalPosition;
		}
}
