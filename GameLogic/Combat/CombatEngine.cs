// using GameData.src.Effect;
// using GameData.src.Effect.Stack;
// using GameData.src.Effect.Status;
// using GameData.src.Effect.Talent;
// using GameData.src.Mob;
// using GameData.src.Player;
// using GameData.src.Shared;
// using GameData.src.Shared.Enums;
// using GameData.src.Shared.Modifier;
// using GameData.src.Shared.Modifiers;
// using GameData.src.Skill;
// using GameData.src.Skill.SkillStep;
// using GameData.src.Stat;
// using GameData.src.Talent.TalentActions;
// using GameLogic.Ports;
// using static GameData.src.Skill.SkillStep.DotDamageStep;

// namespace GameLogic.Combat
// {
//     /** 
//         PreProcess();
//             load characters into combat entities
//             process talent modifiers onto the character

//         PreCombat();
//             process active effects

//         ProcessTurns();
//             apply damage

//         PostCombat();
//             process active effects/dtos at end_turn timing

//         CheckStatus();
//             After Each Turn and Post Combat, check the state of combat

//             Status Applied
//             Damage Done
//             Player Won
//             Player Lost
//             Player Fled
//             Mob Died
//     */
//     public class CombatEngine
//     {
//         internal readonly ISkillRepository skillRepository;
//         internal readonly IEffectRepository effectRepository;
//         internal readonly ITalentRepository talentRepository;

//         private readonly CombatEntity player;
//         private readonly CombatEntity mob;

//         public int playerCurrentHealth;
//         public bool IsCombatActive => combatActive;

//         private bool combatActive = false;
//         private bool intialized = false;
//         private readonly PriorityQueue<CombatEntity, int> turnQueue = new();

//         public CombatEngine(ISkillRepository skillRepository,
//             IEffectRepository effectRepository,
//             ITalentRepository talentRepository,
//             Player player,
//             Mob mob)
//         {
//             this.skillRepository = skillRepository;
//             this.effectRepository = effectRepository;
//             this.talentRepository = talentRepository;
//             // this.player = this.InitializePlayer(player);
//             // this.mob = this.InitializeMob(mob);
//         }

//         // public TurnResult ProcessCombat(CombatCommand command)
//         // {
//         //     var events = new List<CombatEvent>();

//         //     if (!intialized)
//         //     {
//         //         this.combatActive = true;
//         //         this.intialized = true;
//         //         events.Add(new CombatStartEvent());
//         //     }

//         //     if (!this.combatActive)
//         //     {
//         //         events.Add(new CombatEndEvent());
//         //         return new TurnResult(events);
//         //     }

//         //     this.turnQueue.Enqueue(player, player.Stats.Speed);
//         //     this.turnQueue.Enqueue(mob, mob.Stats.Speed);

//         //     do
//         //     {
//         //         var entity = this.turnQueue.Dequeue();

//         //         if (entity.Identifier == player.Identifier)
//         //         {
//         //             switch (command)
//         //             {
//         //                 case UseSkillCommand s:
//         //                     {
//         //                         if (!entity.Skills.TryGetValue(s.SkillId, out var skill))
//         //                         {
//         //                             throw new InvalidOperationException($"Skill with ID '{s.SkillId}' not found.");
//         //                         }

//         //                         if (mob.Identifier != s.TargetId)
//         //                         {
//         //                             throw new InvalidOperationException("identifier does not match any combat entity");
//         //                         }

//         //                         foreach (var combatEvent in ApplyDamage(SelectSkill(entity), entity, player, events))
//         //                         {
//         //                             events.Add(combatEvent);
//         //                         }
//         //                     }

//         //                     break;
//         //                 case FleeCommand:
//         //                     {
//         //                         events.Add(new PlayerFledEvent(true));
//         //                         combatActive = false;
//         //                     }
//         //                     break;
//         //             }
//         //         }
//         //         else
//         //         {
//         //             foreach (var combatEvent in ApplyDamage(SelectSkill(entity), entity, player, events))
//         //             {
//         //                 events.Add(combatEvent);
//         //             }
//         //         }

//         //         foreach (var combatEvent in this.CalculateCombatState(events))
//         //         {
//         //             events.Add(combatEvent);
//         //         }

//         //     } while (this.turnQueue.Count > 0 && combatActive);

//         //     return new TurnResult(events);
//         // }

//         // public static List<CombatEvent> PreCombatTurn(CombatEntity entity, List<CombatEvent> events)
//         // {
//         //     return events;
//         // }

//         // public List<CombatEvent> PostCombatTurn(CombatEntity entity, List<CombatEvent> events)
//         // {
//         //     return events;
//         // }

//         // public static List<CombatEvent> ApplyDamage(SkillCombatDefinition skill,
//         //     CombatEntity source,
//         //     CombatEntity target,
//         //     List<CombatEvent> events)
//         // {
//         //     var hitDamage = skill.Skill.Effects.OfType<HitDamageStep>();
//         //     foreach (var hit in hitDamage)
//         //     {
//         //         var snapshot = new HitDamageSnapshot(hit);

//         //         snapshot.Damage += snapshot.ScaleCoefficient.Stat switch
//         //         {
//         //             StatKind.PhysicalDamage => snapshot.ScaleCoefficient.Operation.ApplyScalarModifier((int)source.Stats.AttackPower),
//         //             StatKind.MeleeDamage => snapshot.ScaleCoefficient.Operation.ApplyScalarModifier((int)source.Stats.AttackPower),
//         //             _ => 0
//         //         };

//         //         foreach (var damageType in snapshot.DamageTypes)
//         //         {
//         //             snapshot.Damage += damageType switch
//         //             {
//         //                 DamageType.Melee => (int)source.Stats.AttackPower,
//         //                 DamageType.Physical => (int)source.Stats.AttackPower,
//         //                 DamageType.Bleed => (int)source.Stats.AttackPower,
//         //                 _ => 0
//         //             };
//         //         }

//         //         if (snapshot.StackFromEffect is not null)
//         //         {
//         //             var effects = source.ActiveEffects.Where(ae => ae.Identifier == snapshot.StackFromEffect.FromEffect).ToList();
//         //             if (effects.Count > 0)
//         //             {
//         //                 snapshot.Damage *= effects.Count;
//         //             }
//         //         }

//         //         foreach (var action in skill.Skill.Effects)
//         //         {
//         //             if (action != skill.Skill.Id)
//         //             {
//         //                 continue;
//         //             }

//         //             if (action.BaseDamage is not null)
//         //             {
//         //                 snapshot.Damage = action.BaseDamage.ApplyScalarModifier(snapshot.Damage);
//         //             }

//         //             snapshot.CanCrit = action.Crit ?? snapshot.CanCrit;

//         //             if (action.DamageTypes is not null)
//         //             {
//         //                 snapshot.DamageTypes = action.DamageTypes.Operation.ApplyCollectionModifier(snapshot.DamageTypes);
//         //             }
//         //         }

//         //         events.Add(new DamageApplied(source.Identifier, target.Identifier, skill.Skill.Id, hit.BaseDamage));
//         //     }

//         //     // var dotDamage = skill.Skill.Effects.OfType<DotDamageStep>();
//         //     // foreach (var dot in dotDamage)
//         //     // {
//         //     //     var snapshot = CombatDotDamageSnapshot.FromStep(dot);
//         //     //     foreach (var action in skill.DotActions)
//         //     //     {
//         //     //         if (action.SkillId != skill.Skill.Id)
//         //     //         {
//         //     //             continue;
//         //     //         }

//         //     //         if (action.BaseDamage is not null)
//         //     //         {
//         //     //             snapshot.BaseDamage = action.BaseDamage.ApplyScalarModifier(snapshot.BaseDamage);
//         //     //         }

//         //     //         if (action.Frequency is not null)
//         //     //         {
//         //     //             snapshot.Frequency = action.Frequency.ApplyScalarModifier(snapshot.Frequency);
//         //     //         }

//         //     //         if (action.Duration is not null)
//         //     //         {
//         //     //             snapshot.Duration = action.Duration.ApplyDurationModifier(snapshot.Duration);
//         //     //         }

//         //     //         if (action.StackStrategy is not null)
//         //     //         {
//         //     //             snapshot.Stacking = action.StackStrategy;
//         //     //         }

//         //     //         if (action.Timing is TimingKind t)
//         //     //         {
//         //     //             snapshot.Timing = t;
//         //     //         }

//         //     //         if (action.DamageTypes is not null)
//         //     //         {
//         //     //             snapshot.DamageTypes = action.DamageTypes.Operation.ApplyCollectionModifier(snapshot.DamageTypes);
//         //     //         }

//         //     //         snapshot.Crit = action.Crit ?? snapshot.Crit;

//         //     //     }

//         //     //     events.Add(new DamageApplied(source.Identifier, target.Identifier, skill.Skill.Id, dot.BaseDamage));
//         //     // }

//         //     return events;
//         // }

//         // public static SkillCombatDefinition SelectSkill(CombatEntity entity)
//         // {
//         //     var random = new Random();
//         //     var skillIndex = random.Next(entity.Skills.Count);
//         //     return entity.Skills.Values.ElementAt(skillIndex);
//         // }

//         // public List<CombatEvent> CalculateCombatState(List<CombatEvent> events)
//         // {
//         //     if (!combatActive)
//         //     {
//         //         return events;
//         //     }

//         //     if (player.CurrentHealth <= 0)
//         //     {
//         //         events.Add(new PlayerDiedEvent());
//         //         events.Add(new CombatEndEvent());

//         //         this.combatActive = false;
//         //     }

//         //     if (mob.CurrentHealth <= 0)
//         //     {
//         //         events.Add(new MobDiedEvent(mob.Identifier));
//         //         this.combatActive = false;
//         //     }

//         //     return events;
//         // }

//         // private CombatEntity InitializePlayer(Player player)
//         // {
//         //     var activeSkills = new Dictionary<string, SkillCombatDefinition>();
//         //     foreach (var entry in player.ClassDefinition.SkillEntry)
//         //     {
//         //         if (entry.Level <= player.Level)
//         //         {
//         //             activeSkills[entry.Id] = new(this.skillRepository.Get(entry.Id));
//         //         }
//         //     }

//         //     var combatEntity = new CombatEntity(nameof(player), player.BaseStats, activeSkills, player.Level);

//         //     var actions = player.Talents.Select(t => t.Actions);

//         //     foreach (var action in actions)
//         //     {
//         //         if (action is ApplyEffectAction ae)
//         //         {
//         //             if (combatEntity.Skills.TryGetValue(ae.FromSkill, out var skill))
//         //             {
//         //                 skill.Effects[ae.Effect] = this.effectRepository.Get(ae.Effect);
//         //             }
//         //         }
//         //         else if (action is ModifyHitDamageAction hd)
//         //         {
//         //             if (combatEntity.Skills.TryGetValue(hd.Skill, out var skill))
//         //             {
//         //                 skill.HitActions.Add(hd);
//         //             }
//         //         }
//         //         else if (action is ModifyDotDamageAction dd)
//         //         {
//         //             if (combatEntity.Skills.TryGetValue(dd.SkillId, out var skill))
//         //             {
//         //                 skill.DotActions.Add(dd);
//         //             }
//         //         }
//         //     }

//         //     return combatEntity;
//         // }

//         // private CombatEntity InitializeMob(Mob mob)
//         // {
//         //     var skills = new Dictionary<string, SkillCombatDefinition>();
//         //     foreach (var skillId in mob.Skills)
//         //     {
//         //         skills[skillId] = new(this.skillRepository.Get(skillId));
//         //     }

//         //     return new CombatEntity(nameof(mob), mob.BaseStats, skills, mob.Level);
//         // }


//         /// Calculates hit damage in the following order:
//         /// 1. Source Stat Scaling
//         /// 2. Additive Scalars
//         ///    - Modify Hit Scaling
//         ///    - Modify Skill Scaling
//         ///    - Modify Effect Scaling
//         /// 3. Multiplicative Scalars
//         ///    - Modify Hit Scaling
//         ///    - Modify Skill Scaling
//         ///    - Modify Effect Scaling
//         ///    - Target Effect Scaling
//         /// 4. Target Scaling and Reductions
//         ///    - Target Additive Effect Scaling
//         ///    - Target Multiplicative Effect Scaling
//         ///    - Target Stat Defenses
//         private int CalculateHitDamage(SkillCombatDefinition skill, CombatEntity source, CombatEntity target)
//         {
//             var hitStep = skill.Skill.Effects.OfType<HitDamageStep>();

//             foreach (var step in hitStep)
//             {
//                 var multiplicateModifier = 0f;
//                 var additiveModifier = 0;

//                 var snapshot = new HitDamageSnapshot(step);
//                 {
//                     // Process Stat Scaling
//                     // if (snapshot.ScaleCoefficient.Stat is StatKind.PhysicalDamage or StatKind.MeleeDamage)
//                     // {
//                     //     if (snapshot.ScaleCoefficient.Operation.ModifierOperation == ScalarOperation.OperationKind.Add)
//                     //     {
//                     //         additiveModifier += snapshot.ScaleCoefficient.Operation.Value;
//                     //     }
//                     //     else if (snapshot.ScaleCoefficient.Operation.ModifierOperation == ScalarOperation.OperationKind.Mult)
//                     //     {
//                     //         multiplicateModifier += snapshot.ScaleCoefficient.Operation.Value;
//                     //     }
//                     // }
//                 }

//                 //Process Modifiers
//                 foreach (var mod in source.Modifiers)
//                 {
//                     {
//                         if (mod is ModifyHitDamageAction action && action.SkillId == skill.Skill.Id)
//                         {
//                             if (action.BaseDamage?.ModifierOperation is ScalarOperationOld.OperationKind.Add)
//                             {
//                                 additiveModifier += action.BaseDamage.Value;
//                             }
//                             else if (action.BaseDamage?.ModifierOperation is ScalarOperationOld.OperationKind.Mult)
//                             {
//                                 multiplicateModifier += action.BaseDamage.Value;
//                             }
//                         }
//                     }
//                 }

//                 //Process Effects
//                 var (addScaling, multScaling) = ProcessEffectHitScaling(snapshot, skill.Skill.Id, source.ActiveEffects);
//                 additiveModifier += addScaling;
//                 multiplicateModifier += multScaling;

//                 var (ar, mr) = ProcessEffectHitScaling(snapshot, skill.Skill.Id, target.ActiveEffects);
//                 additiveModifier -= ar;
//                 multiplicateModifier -= mr;

//                 snapshot.Damage += additiveModifier;
//                 snapshot.Damage = (int)(snapshot.Damage * (1 + multiplicateModifier));
//             }

//             return 0;
//         }

//         private static (int additive, float multiplicative) ProcessEffectHitScaling(HitDamageSnapshot snapshot, string skillId, List<EffectSnapshot> effects)
//         {
//             var additive = 0;
//             var multiplicative = 0f;
//             foreach (var effect in effects)
//             {
//                 foreach (var modifier in effect.Modifiers)
//                 {
//                     {
//                         if (modifier is AttackModifer am)
//                         {
//                             if (am.Operation.ModifierOperation == ScalarOperationOld.OperationKind.Add)
//                             {
//                                 additive += am.Operation.Value;
//                             }
//                             else if (am.Operation.ModifierOperation == ScalarOperationOld.OperationKind.Mult)
//                             {
//                                 multiplicative += am.Operation.Value;
//                             }
//                         }

//                         if (modifier is DamageModifer dm && snapshot.DamageTypes.Contains(dm.DamageType))
//                         {
//                             if (dm.Operation.ModifierOperation == ScalarOperationOld.OperationKind.Add)
//                             {
//                                 additive += dm.Operation.Value;
//                             }
//                             else if (dm.Operation.ModifierOperation == ScalarOperationOld.OperationKind.Mult)
//                             {
//                                 multiplicative += dm.Operation.Value;
//                             }
//                         }

//                         if (modifier is SkillModifier sm && skillId == sm.SkillId)
//                         {
//                             if (sm.Operation.ModifierOperation == ScalarOperationOld.OperationKind.Add)
//                             {
//                                 additive += sm.Operation.Value;
//                             }
//                             else if (sm.Operation.ModifierOperation == ScalarOperationOld.OperationKind.Mult)
//                             {
//                                 multiplicative += sm.Operation.Value;
//                             }
//                         }
//                     }
//                 }
//             }

//             return (additive, multiplicative);
//         }

//         private DotEffectSnapshot ApplyDotEffectSnapshot(SkillDefinition definition, CombatEntity source, CombatEntity target)
//         {
//             var dotSteps = definition.Effects.OfType<DotDamageStep>();
//             foreach (var dotStep in dotSteps)
//             {
//                 var snapshot = new DotEffectSnapshot(dotStep);
//                 return snapshot;
//             }

//             throw new NotImplementedException();
//         }

//         private int CalculateDotDamage(DotEffectSnapshot snapshot, CombatEntity source, CombatEntity target)
//         {
//             return 0;
//         }

//         private int ApplyEffect(SkillDefinition skill, CombatEntity source, CombatEntity target)
//         {
//             return 0;
//         }

//         public class CombatEntity
//         {
//             public bool IsPlayer { get; }
//             public string Identifier { get; }
//             private int currentHealth;
//             public int MaxHealth { get; }

//             public int CurrentHealth
//             {
//                 get => currentHealth;
//                 set => currentHealth = Math.Max(0, value);
//             }

//             public IReadOnlyList<ITalentAction> Modifiers { get; }

//             public List<EffectSnapshot> ActiveEffects = [];
//             public Dictionary<string, SkillCombatDefinition> Skills { get; }

//             public StatSnapshot Stats { get; }

//             int Level { get; }

//             public CombatEntity(string identifier,
//                 StatSnapshot stats,
//                 Dictionary<string, SkillCombatDefinition> skills,
//                 IReadOnlyList<ITalentAction> modifiers,
//                 int level,
//                 bool isPlayer = false)
//             {
//                 this.Stats = stats;
//                 this.Skills = skills;
//                 this.Level = level;
//                 this.Identifier = identifier;
//                 this.Modifiers = modifiers;
//             }
//         }

//         public class SkillCombatDefinition
//         {
//             // public List<ModifySkillAction> skillActions { get; } = [];
//             // public List<ModifyEffectAction> activeEffectModifiers { get; } = [];
//             public Dictionary<string, ISkillStep> Effects { get; } = [];
//             // public List<ModifyDotDamageAction> DotActions { get; } = [];
//             // public List<ModifyHitDamageAction> HitActions { get; } = [];
//             public SkillDefinition Skill { get; }

//             public SkillCombatDefinition(SkillDefinition skill)
//             {
//                 Skill = skill;
//             }
//         }

//         public struct StatSnapshot()
//         {
//             public int MeleeDamage;
//             public int SpellDamage;
//             public int PhysicalDamage;
//             public int ElementalDamage;
//             public int Defense;
//             public int Avoidance;
//             public int Speed;
//             public int Ward;
//             public int Health;

//             public StatSnapshot(StatTemplate stats) : this()
//             {
//                 if (stats.Stats.TryGetValue(StatKind.MeleeDamageAdded, out var md))
//                 {
//                     MeleeDamage = md;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.SpellDamageAdded, out var sd))
//                 {
//                     SpellDamage = sd;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.PhysicalDamageAdded, out var pd))
//                 {
//                     PhysicalDamage = pd;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.ElementalDamageAdded, out var ed))
//                 {
//                     ElementalDamage = ed;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.ArmorRatingAdded, out var def))
//                 {
//                     Defense = def;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.AvoidanceRatingAdded, out var av))
//                 {
//                     Avoidance = av;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.SpeedRatingAdded, out var sp))
//                 {
//                     Speed = sp;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.WardRatingAdded, out var wa))
//                 {
//                     Ward = wa;
//                 }
//                 if (stats.Stats.TryGetValue(StatKind.HealthRatingAdded, out var hp))
//                 {
//                     Health = hp;
//                 }
//             }
//         }

//         public struct HitDamageSnapshot(HitDamageStep d)
//         {
//             public List<DamageType> DamageTypes { get; set; } = [.. d.DamageTypes];

//             public int Damage { get; set; } = d.BaseDamage;

//             public bool CanCrit { get; set; } = d.Crit;

//             public StackFromEffect? StackFromEffect { get; set; } = d.StackFromEffect;
//         }

//         public struct DotEffectSnapshot(DotDamageStep d)
//         {
//             public List<DamageType> DamageTypes { get; set; } = [.. d.DamageTypes];

//             public int Damage { get; set; } = d.BaseDamage;

//             public Duration Duration { get; set; } = d.Duration;

//             public int Frequency { get; set; } = d.Frequency;

//             public IStackStrategy StackingStrategy { get; set; } = d.Stacking;

//             public bool CanCrit { get; set; } = d.Crit;
//         }

//         public struct EffectSnapshot(EffectDefinition effect)
//         {
//             public readonly string Identifier => effect.Id;
//             public EffectCategory Category { get; set; } = effect.Category;

//             public Duration Duration { get; set; } = effect.Duration;

//             public IStackStrategy Stacking { get; set; } = effect.StackStrategy;

//             public List<ModifierBase> Modifiers { get; init; } = [.. effect.Modifiers];

//             public int Leech { get; init; } = effect.Leech;

//             public List<DamageType> DamageTypes { get; init; } = [.. effect.DamageTypes];

//             public List<IStatus> ApplyStatus { get; init; } = [.. effect.ApplyStatus];
//         }

//         public abstract record CombatEvent;
//         public record CombatStartEvent : CombatEvent;
//         public record CombatEndEvent : CombatEvent;
//         public record PlayerDiedEvent : CombatEvent;
//         public record PlayerFledEvent(bool Fled) : CombatEvent;
//         public record MobDiedEvent(string TargetId) : CombatEvent;
//         public record DamageApplied(string SourceId, string TargetId, string SkillId, int Amount) : CombatEvent;
//         public record EffectApplied(string SourceId, string TargetId, string EffectId) : CombatEvent;
//         public record StatusApplied(IStatus Status, string SourceId) : CombatEvent;

//         public readonly struct TurnResult(IReadOnlyList<CombatEvent> events)
//         {
//             public IReadOnlyList<CombatEvent> Events => events;
//         }

//         public abstract record CombatCommand;
//         public record UseSkillCommand(string SkillId, string TargetId) : CombatCommand;
//         public record FleeCommand : CombatCommand;

//     }
// }