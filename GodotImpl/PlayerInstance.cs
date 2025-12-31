using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TopDownGame
{
  internal class PlayerInstance
  {
	public float MaxHealth { get; set; } = 200f;
	public float CurrentHealth { get; set; } = 200f;
	public float MinHealth { get; set; } = 0f;

	public float CurrentEnergy { get; set; } = 100f;
	public float MaxEnergy { get; set; } = 100f;
	public float MinEnergy { get; set; } = 0f;
  }
}
