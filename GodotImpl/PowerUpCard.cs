using Godot;

public partial class PowerUpCard : ColorRect
{
		private const string DescriptionFormat = "Increases {0} by {1}%.";
		[Export]
		public Label Title;

		[Export]
		public Label Description;

		[Export]
		public ColorRect ColorRect;


		public override void _Ready()
		{
		}
}
