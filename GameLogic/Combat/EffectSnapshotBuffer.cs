using System.Collections.Immutable;
using GameData.src.Skill.SkillStep;
using GameLogic.Combat.Snapshots;

namespace GameLogic.Combat
{
    public class EffectSnapshotBuffer
    {
        private readonly List<EffectSnapshot> Effects = new();

        private int effectCount = 0;

        public bool HasEffect => this.effectCount > 0;

        public EffectSnapshotBuffer(IReadOnlyList<EffectSnapshot> effects)
        {
            for (int i = 0; i < effects.Count; i++)
            {
                this.Effects.Add(effects[i]);
            }
        }

        public EffectSnapshotBuffer() { }

        public List<EffectSnapshot> GetEffects()
        => this.Effects;

        public bool AddEffect(EffectSnapshot effect)
        {
            var merged = false;
            for (int i = 0; i < this.Effects.Count; i++)
            {
                var effectSnapshot = this.Effects[i];

                if (effect.EffectId == effectSnapshot.EffectId)
                {
                    if (effectSnapshot.StackStrategy.RefreshMode == GameData.src.Effect.Stack.StackRefreshMode.ResetTime)
                    {
                        effectSnapshot.StackStrategy.StackCount
                            = Math.Min(effect.StackStrategy.StackCount
                                + effect.StackStrategy.StacksPerApplication,
                                effect.StackStrategy.MaxStacks);

                        merged = true;
                        break;
                    }
                }

                if (!merged)
                {
                    this.Effects.Add(effect);
                }
            }

            return false;
        }

        public void RemoveEffect(EffectSnapshot effect)
        {
            for (int i = 0; i < this.Effects.Count; i++)
            {
                if (this.Effects[i].Equals(effect))
                {
                    this.Effects[i] = default(EffectSnapshot);
                    --this.effectCount;

                    return;
                }
            }
        }
    }
}