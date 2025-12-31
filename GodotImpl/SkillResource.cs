using Godot;

namespace GodotImpl;

[GlobalClass]
public abstract partial class SkillResource : Resource
{
		public abstract ISkill Skill { get; }

		public abstract string IconPath { get; }
}
