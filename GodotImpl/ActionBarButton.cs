using Godot;
using System;

public partial class ActionBarButton : Panel
{
	[Export] public string Title;
	[Export] public Texture2D ActionIcon;

	[Export]
	public float Cooldown;

	private Label _label;

  public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		_label.Text = Title;
  }
}
