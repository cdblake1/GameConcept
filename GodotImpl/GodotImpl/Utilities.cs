using Godot;

#nullable enable

namespace GodotImpl
{
		internal static class Utilities
		{
				public static Node2D? FindPlayer(Node node)
				{
						if (node == null)
								return null;

						// Try by group first
						var player = node.GetTree().Root.FindChild(pattern: "Player",
								recursive: true,
								owned: false);

						if (player != null && player is Node2D p0)
								return p0;

						return null;
				}
		}
}
