using System.Collections.Immutable;
using GameData.src.Skill.SkillStep;
using GameLogic.Combat.Snapshots;

namespace GameLogic.Combat
{
    public class EffectSnapshotBuffer
    {
        private readonly Memory<EffectSnapshot> Effects = new EffectSnapshot[Globals.MaxEffects];

        private int effectCount = 0;

        public bool HasEffect => this.effectCount > 0;

        public EffectSnapshotBuffer(IReadOnlyList<EffectSnapshot> effects)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                this.Effects.Span[i] = effects[i];
            }
        }

        public EffectSnapshotBuffer() { }

        public ReadOnlySpan<EffectSnapshot> GetEffects()
        => this.Effects.Span;

        public bool AddEffect(EffectSnapshot effect)
        {
            for (int i = 0; i < this.Effects.Length; i++)
            {
                if (this.Effects.Span[i].Equals(default(EffectSnapshot)))
                {
                    this.Effects.Span[i] = effect;
                    this.effectCount++;
                    return true;
                }

            }

            return false;
        }

        public void RemoveEffect(EffectSnapshot effect)
        {
            for (int i = 0; i < this.Effects.Length; i++)
            {
                if (this.Effects.Span[i].Equals(effect))
                {
                    this.Effects.Span[i] = default(EffectSnapshot);
                    --this.effectCount;

                    return;
                }
            }
        }
    }
}