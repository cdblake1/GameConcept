using Godot;

namespace GodotImpl;

public partial class RangeIndicator : Node2D
{
		[Export] public float Radius = 128f;          // in world pixels/units
		[Export] public Color RingColor = new(1, 1, 1, 0.8f);
		[Export] public float Thickness = 2f;         // line width
		[Export] public int Segments = 64;            // smoothness
		[Export] public bool Dashed = true;          // optional

		public override void _Ready()
		{
				QueueRedraw(); // draw once
		}

		public override void _Notification(int what)
		{
				if (what == NotificationTransformChanged) QueueRedraw(); // redraw if moved/scaled
		}

		public override void _Draw()
		{
				// We draw at this node's local origin (0,0) — keep it centered on the player.
				if (!Dashed)
				{
						DrawArc(Vector2.Zero, Radius, 0, Mathf.Tau, Segments, RingColor, Thickness, true);
				}
				else
				{
						// simple dashed arc: draw short segments around the circle
						float dashLen = Mathf.Pi / 48f; // angular length per dash
						float gapLen = dashLen;        // same-sized gaps
						float angle = 0f;
						while (angle < Mathf.Tau)
						{
								float a0 = angle;
								float a1 = Mathf.Min(angle + dashLen, Mathf.Tau);
								DrawArc(Vector2.Zero, Radius, a0, a1, 4, RingColor, Thickness, true);
								angle += dashLen + gapLen;
						}
				}
		}

		// Call this if you change Radius/Color/Thickness at runtime
		public void SetRadius(float r) { Radius = r; QueueRedraw(); }
		public void SetColor(Color c) { RingColor = c; QueueRedraw(); }
		public void SetThickness(float t) { Thickness = t; QueueRedraw(); }
}
