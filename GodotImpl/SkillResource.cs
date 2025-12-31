using Godot;
using TopDownGame.Skill;

namespace TopDownGame
{
  [GlobalClass]
  public abstract partial class SkillResource : Resource
  {
	public abstract ISkill Skill { get; }

	public abstract string IconPath { get; }
  }
}
