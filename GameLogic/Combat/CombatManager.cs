using GameData;

namespace GameLogic.Combat
{
    public class CombatManager
    {
        private readonly IReadOnlyList<CharacterBase> players;
        private readonly IReadOnlyList<MobBase> mobs;
        private readonly EffectProcessor effectApplicator = new EffectProcessor();

        private PriorityQueue<ActorBase, int> turnQueue = new PriorityQueue<ActorBase, int>();

        private bool combatStarted = false;

        public CombatManager(IReadOnlyList<CharacterBase> players, IReadOnlyList<MobBase> mobs)
        {
            this.players = players ?? throw new ArgumentNullException(nameof(players));
            this.mobs = mobs ?? throw new ArgumentNullException(nameof(mobs));
        }

        public IEnumerable<CombatEvent> InitializeTurn()
        {
            if (!combatStarted)
            {
                combatStarted = true;
                yield return new CombatStartEvent(players, mobs);
            }

            SetUpTurnQueue();

            if (turnQueue.Count == 0)
            {
                throw new InvalidOperationException("No actors available for combat.");
            }

            if (!players.Where(p => p.CurrentHealth > 0).Any())
            {
                combatStarted = false;
                yield return new CombatEndEvent(false); // Players lost
                yield break;
            }

            if (!mobs.Where(m => m.CurrentHealth > 0).Any())
            {
                combatStarted = false;
                yield return new CombatEndEvent(true); // Players won
                yield break;
            }

            // Apply effects to all living actors
            foreach (var actor in GetAllLivingActors())
            {
                foreach (var effect in actor.Effects)
                {
                    yield return effectApplicator.ApplyEffect(actor, effect);
                }
            }
        }

        private void SetUpTurnQueue()
        {
            turnQueue.Clear();

            foreach (var actor in GetAllLivingActors())
            {
                System.Diagnostics.Debugger.Launch();
                turnQueue.Enqueue(actor, actor.Stats.Speed);
            }
        }

        private IEnumerable<ActorBase> GetAllLivingActors()
        {
            return players.Where(p => p.CurrentHealth > 0)
                         .Cast<ActorBase>()
                         .Concat(mobs.Where(m => m.CurrentHealth > 0));
        }

        public IEnumerable<ActorBase> GetNextActor()
        {
            while (turnQueue.Count > 0)
            {
                yield return turnQueue.Dequeue();
            }
        }

        public IEnumerable<CombatEvent> ExecuteCommand(CombatCommand command)
        {
            if (!combatStarted)
                throw new InvalidOperationException("Combat has not started yet.");

            List<CombatEvent> events = [];

            switch (command)
            {
                case UseSkillCommand skillCommand:
                    var effects = skillCommand.Skill.Apply(new SkillPropertySnapshot()
                    {
                        AddedBaseDamage = skillCommand.Caster.Stats.AttackPower,
                        AddedBaseDamageMultiplier = 1.0,
                        Multiplier = 1.0
                    });

                    foreach (var evt in effectApplicator.ApplyEffects(skillCommand.Target, effects))
                    {
                        events.Add(evt);
                        yield return evt;
                    }

                    break;

                case UseItemCommand:
                    throw new NotImplementedException("UseItemCommand is not implemented yet.");

                case FleeCommand fleeCommand:
                    var fleeEvent = new FleeEvent(fleeCommand.Actor, true); // Placeholder logic
                    yield return fleeEvent;
                    yield break;

                default:
                    throw new InvalidOperationException("Unknown combat command.");
            }

            // Post-processing: check for death
            foreach (var evt in PostCombatChecks())
                yield return evt;
        }

        public bool IsCombatOngoing() => combatStarted;


        private IEnumerable<CombatEvent> PostCombatChecks()
        {
            foreach (var player in players)
            {
                if (player.CurrentHealth <= 0)
                    yield return new ActorDefeatedEvent(player);
            }

            foreach (var mob in mobs)
            {
                if (mob.CurrentHealth <= 0)
                    yield return new ActorDefeatedEvent(mob);
            }

            // Combat end condition
            bool allPlayersDead = !players.Any(p => p.CurrentHealth > 0);
            bool allMobsDead = !mobs.Any(m => m.CurrentHealth > 0);

            if (allPlayersDead)
            {
                combatStarted = false;
                yield return new CombatEndEvent(false); // Players lost
            }
            else if (allMobsDead)
            {
                combatStarted = false;
                yield return new CombatEndEvent(true); // Players won
            }
        }

    }

    public abstract record CombatCommand
    {

    }

    public record UseSkillCommand(ActorBase Caster, ActorBase Target, Skill Skill) : CombatCommand;
    public record UseItemCommand(ActorBase User, IItem Item) : CombatCommand;
    public record FleeCommand(ActorBase Actor) : CombatCommand;

    public abstract record CombatEvent { }

    public record CombatStartEvent(IReadOnlyList<CharacterBase> Players, IReadOnlyList<MobBase> Mobs) : CombatEvent;

    public record StatusEffectEvent(StatusEffect Effect, ActorBase Target) : CombatEvent;

    public record DamageEvent(DamageEffect Effect, ActorBase Target, double Damage) : CombatEvent;

    public record CombatEndEvent(bool playerWon) : CombatEvent;

    public record FleeEvent(ActorBase Actor, bool success) : CombatEvent;

    public record ActorDefeatedEvent(ActorBase Actor) : CombatEvent;

    public class EffectProcessor
    {
        public IEnumerable<CombatEvent> ApplyEffects(ActorBase target, IReadOnlyList<Effect> effects)
        {
            foreach (var effect in effects)
            {
                yield return ApplyEffect(target, effect);
            }
        }

        public CombatEvent ApplyEffect(ActorBase target, Effect effect)
        {
            return effect switch
            {
                StatusEffect statusEffect => ApplyStatusEffect(target, statusEffect),
                DamageEffect damageEffect => ApplyDamageEffect(target, damageEffect),
                _ => throw new InvalidOperationException($"Unknown effect type: {effect.GetType().Name}")
            };
        }

        private CombatEvent ApplyStatusEffect(ActorBase target, StatusEffect statusEffect)
        {
            target.ApplyEffect([statusEffect]);
            return new StatusEffectEvent(statusEffect, target);
        }

        private CombatEvent ApplyDamageEffect(ActorBase target, DamageEffect damageEffect)
        {
            var damage = target.ApplyDamage(damageEffect);
            return new DamageEvent(damageEffect, target, damage);
        }
    }
}