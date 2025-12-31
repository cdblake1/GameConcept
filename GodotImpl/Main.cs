using Godot;

public partial class Main : Node
{

  [Export]
  private PackedScene SkeletonOneScene;

  public void OnMobTimerTimeout()
  {
    SkeletonOne skeleton = SkeletonOneScene.Instantiate<SkeletonOne>();

    var mobSpawnLocation = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    mobSpawnLocation.ProgressRatio = GD.Randf();

    float direction = mobSpawnLocation.Rotation + Mathf.Pi / 2;

    skeleton.Position = mobSpawnLocation.Position;

    direction += (float)GD.RandRange(-Mathf.Pi / 4, Mathf.Pi / 4);
    skeleton.Rotation = direction;

    var velocity = new Vector2((float)150, 0);
    skeleton.Velocity = velocity;

    AddChild(skeleton);
  }
}
