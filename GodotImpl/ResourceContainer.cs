using Godot;
using System;

public partial class ResourceContainer : HBoxContainer
{
		[Export]
		private float _currentValue;

		[Export]
		private float _maxValue;

		[Export]
		private float _minValue = 0;

		[Export]
		private Color _barColor = Colors.Transparent;

		[Export]
		private ProgressBar _progressBar;

		[Export]
		private Label _valueLabel;

		[Export]
		private float height = 20;

		[Export]
		private float width = 150;

		[Export]
		private bool hideLabel = false;

		public override void _Ready()
		{
				_progressBar.MinValue = _minValue;
				_progressBar.MaxValue = _maxValue;
				_progressBar.Value = _currentValue;

				BarColor = _barColor;

				_valueLabel.Text = $"{_currentValue}";

				_progressBar.CustomMinimumSize = new Vector2(width, height);
				_valueLabel.Visible = !hideLabel;
		}

		public float CurrentValue
		{
				get => _currentValue;
				set
				{
						_currentValue = Math.Clamp(value, _minValue, _maxValue);
						_progressBar.Value = _currentValue;
						_valueLabel.Text = $"{_currentValue}";
				}
		}

		public float MaxValue
		{
				get => _maxValue;
				set
				{
						_maxValue = value;
						_progressBar.MaxValue = _maxValue;
						CurrentValue = Math.Clamp(_currentValue, _minValue, _maxValue);
				}
		}

		public float MinValue
		{
				get => _minValue;
				set
				{
						_minValue = value;
						_progressBar.MinValue = _minValue;
						CurrentValue = Math.Clamp(_currentValue, _minValue, _maxValue);
				}
		}

		public Color BarColor
		{
				get => _barColor;
				set
				{
						_barColor = value;
						var fillStyleBox = _progressBar.GetThemeStylebox("fill") as StyleBoxFlat;
						if (fillStyleBox != null)
						{
								fillStyleBox.BgColor = _barColor;
						}
				}
		}
}
