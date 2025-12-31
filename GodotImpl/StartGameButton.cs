using Godot;
using System;

public partial class StartGameButton : Button
{
  public override void _Pressed()
  {
	GetTree().ChangeSceneToFile("res://GameScene.tscn");
	base._Pressed();
  }
}
