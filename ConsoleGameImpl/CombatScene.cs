
using GameData;
using GameData.src.Item;
using GameLogic.Combat;

class CombatScene(CharacterBase player, MobBase mob)
{
    private readonly CharacterBase player = player ?? throw new ArgumentNullException(nameof(player));
    private readonly MobBase mob = mob ?? throw new ArgumentNullException(nameof(mob));
    private readonly GameTextPrinter textPrinter = new();

    private readonly CombatManager combatManager = new CombatManager(
        [player], [mob]);

    private readonly Menu menu = new("Combat Menu",
    [
        new MenuOption("Attack", ConsoleKey.A),
        new MenuOption("Check Consumables", ConsoleKey.C),
        new MenuOption("Run"),
    ]);

    public CombatOutcome StartScene()
    {
        EnterCombat();

        var outcome = GetCombatOutcome();

        if (CombatOutcome.Victory == outcome)
        {
            var experienceReward = mob.AwardExperience();

            DialogueQueue.AddDialogue(
                [[
                    new TextPacket($"You gained"),
                    new TextPacket($" {experienceReward}", GameTextPrinter.GetColor(TextKind.Experience)),
                    new TextPacket(" experience points.")
                ]]);

            var leveled = this.player.LevelManager.AddExperience(experienceReward);

            if (leveled)
            {
                DialogueQueue.AddDialogue(
                    [
                        [new TextPacket("You leveled up!")],
                        [
                            new TextPacket($" You are now level "),
                            new TextPacket($"{this.player.LevelManager.CurrentLevel}", GameTextPrinter.GetColor(TextKind.LevelUp)),
                            new TextPacket("!")
                        ]
                    ]);

                this.textPrinter.WaitForInput();
            }
            else
            {
                DialogueQueue.AddDialogue([[
                    new("You are "),
                    new($"{this.player.LevelManager.GetExperienceNeededForNextLevel() - this.player.LevelManager.CurrentExperience} ", GameTextPrinter.GetColor(TextKind.Experience)),
                    new("experience away from levelling.")
                ]]);
            }

            var loot = mob.DropLoot();
            if (loot != null)
            {
                player.Inventory.AddItem(loot);

                if (loot is CraftingMaterial material)
                {
                    textPrinter.Print([
                        new TextPacket("You received"),
                        new TextPacket($" {material.Count}"),
                        new TextPacket($" {material.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
                    ]);

                    textPrinter.WaitForInput();
                }
                else if (loot is Equipment equipment)
                {
                    textPrinter.Print([
                        new TextPacket("You received "),
                        new TextPacket($" {equipment.Name}", GameTextPrinter.GetColor(TextKind.LootRare)),
                        new TextPacket("!")
                    ]);

                    textPrinter.WaitForInput();
                }
                else
                {
                    textPrinter.Print([
                        new TextPacket("You received "),
                        new TextPacket($" {loot.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
                        new TextPacket("!")
                    ]);

                    textPrinter.WaitForInput();
                }
            }
            else
            {
                textPrinter.Print("No loot dropped.");
                textPrinter.WaitForInput();
            }

        }
        else if (CombatOutcome.Defeat == outcome)
        {
            this.textPrinter.Print(
                [
                    new TextPacket("You have lost!"),
                    new TextPacket("You have been defeated and will respawn at the last checkpoint.")
                ]);

            this.textPrinter.WaitForInput();
        }
        else if (CombatOutcome.Flee == outcome)
        {
            this.textPrinter.Print(
                [
                    new TextPacket("You fled!")
                ]);

            this.textPrinter.WaitForInput();
        }

        return outcome;
    }

    public void EnterCombat()
    {
        do
        {
            PreCombat();

            var input = this.menu.ShowMenu();
            Console.Clear();

            switch (input)
            {
                case 0:
                    Attack();
                    break;
                case 1:
                    this.textPrinter.Print(
                        [
                            new TextPacket("You check your consumables."),
                            new TextPacket("You have:"),
                            new TextPacket($"0 consumables")
                        ]);

                    this.textPrinter.WaitForInput();
                    break;
                case 2:
                    RunAway();
                    return;
                default:
                    break;
            }
        } while (this.combatManager.IsCombatOngoing());
    }

    public void Attack()
    {
        foreach (var actor in this.combatManager.GetNextActor())
        {
            if (!combatManager.IsCombatOngoing())
            {
                return;
            }

            if (actor is CharacterBase character)
            {
                var command = PlayerAttack(character, this.mob);
                foreach (var evt in this.combatManager.ExecuteCommand(command))
                {
                    RenderEvent(evt);
                }
            }
            else if (actor is MobBase mob)
            {
                var commandTwo = MobAttack(mob, player);
                foreach (var evt in this.combatManager.ExecuteCommand(commandTwo))
                {
                    RenderEvent(evt);
                }
            }
            else
            {
                throw new InvalidOperationException("Unknown actor type.");
            }
        }
    }

    private void PreCombat()
    {
        foreach (var evt in this.combatManager.InitializeTurn())
        {
            RenderEvent(evt);
        }
    }

    private void RenderEvent(CombatEvent evt)
    {
        switch (evt)
        {
            case CombatStartEvent startEvent:
                List<List<TextPacket>> startMessages = [];
                foreach (var mob in startEvent.Mobs)
                {
                    startMessages.Add([
                        new TextPacket($"A "),
                        PrintMobName(mob, startEvent.Players[0]),
                        new(" has appeared!")]);
                }

                startMessages.Add([new TextPacket("Prepare for combat!")]);

                DialogueQueue.AddDialogue([.. startMessages]);
                break;
            case CombatEndEvent endEvent:
                if (endEvent.playerWon)
                {
                    DialogueQueue.AddDialogue([[new("You have defeated "), PrintMobName(mob, player), new("!")]]);
                }
                else
                {
                    DialogueQueue.AddDialogue([[
                        new TextPacket("You have lost to "),
                        PrintMobName(mob, player),
                        new TextPacket("!")
                    ]]);
                }

                return;
            case DamageEvent damageEvent:
                if (damageEvent.Target is CharacterBase character)
                {
                    DialogueQueue.AddDialogue([
                        [
                            new TextPacket($"{character.Name} took "),
                            PrintDamage((int)damageEvent.Damage),
                            new TextPacket(" damage!")
                        ],
                        [
                            new TextPacket($"{character.Name} has "),
                            new($"{character.CurrentHealth}", GameTextPrinter.GetColor(TextKind.Health)),
                            new(" HP left.")
                        ]
                    ]);
                }
                else if (damageEvent.Target is MobBase mob)
                {
                    DialogueQueue.AddDialogue([
                        [
                            PrintMobName(mob, this.player),
                            new TextPacket(" took "),
                            PrintDamage((int)damageEvent.Damage),
                            new TextPacket(" damage!")
                        ],
                        [
                            PrintMobName(mob, this.player),
                            new TextPacket(" has "),
                            new($"{mob.CurrentHealth}", GameTextPrinter.GetColor(TextKind.Health)),
                            new(" HP left.")
                        ]
                    ]);
                }
                break;
            case StatusEffectEvent statusEffectEvent:
                if (statusEffectEvent.Target is CharacterBase chartwo)
                {
                    this.textPrinter.Print(
                        [
                            new TextPacket($"{chartwo.Name} is affected by "),
                                PrintStatusName(statusEffectEvent.Effect),
                                new TextPacket(".")
                        ]);

                    this.textPrinter.WaitForInput();
                }
                else if (statusEffectEvent.Target is MobBase mob)
                {
                    this.textPrinter.Print(
                        [
                            PrintMobName(mob, this.player),
                                new TextPacket(" is affected by "),
                                PrintStatusName(statusEffectEvent.Effect),
                                new TextPacket(".")
                        ]);

                    this.textPrinter.WaitForInput();
                }
                break;
            case FleeEvent fleeEvent:
                if (fleeEvent.success)
                {
                    this.textPrinter.Print(
                        [
                            new TextPacket($"{fleeEvent.Actor.Name} successfully fled the combat!"),
                        ]);

                    this.textPrinter.WaitForInput();
                }
                else
                {
                    this.textPrinter.Print(
                        [
                            new TextPacket($"{fleeEvent.Actor.Name} failed to flee the combat!"),
                        ]);

                    this.textPrinter.WaitForInput();
                }
                break;
            case ActorDefeatedEvent defeatedEvent:
                if (defeatedEvent.Actor is CharacterBase charThree)
                {
                    this.textPrinter.Print(
                        [
                            new TextPacket($"{charThree.Name} has been defeated!"),
                        ]);

                    this.textPrinter.WaitForInput();
                }
                else if (defeatedEvent.Actor is MobBase mob)
                {
                    this.textPrinter.Print(
                        [
                            PrintMobName(mob, this.player),
                            new TextPacket(" has been defeated!"),
                        ]);

                    this.textPrinter.WaitForInput();
                }
                break;
        }
    }
    private void RunAway()
    {
        var command = new FleeCommand(this.player);
        foreach (var evt in this.combatManager.ExecuteCommand(command))
        {
            RenderEvent(evt);
        }
    }

    private CombatCommand MobAttack(MobBase mob, CharacterBase target)
    {
        var skill = mob.AttackSkill[0];

        return new UseSkillCommand(mob, target, skill);
    }

    private TextPacket PrintStatusName(StatusEffect effect)
    {
        return new TextPacket($"{effect.Name}", GameTextPrinter.GetColor(TextKind.StatusEffect));
    }

    private TextPacket PrintDamage(int damage)
    {
        return new TextPacket($"{damage}", GameTextPrinter.GetColor(TextKind.Damage));
    }

    private CombatCommand PlayerAttack(CharacterBase player, MobBase mob)
    {
        var availableSkills = LoadAvailableSkills();
        var menuOptions = availableSkills.Select(skill => new MenuOption(skill.Name)).ToArray();
        var skillMenu = new Menu("Select Skill", menuOptions);

        int input;
        do
        {
            input = skillMenu.ShowMenu();
        } while (input == -1 || input < 0 || input >= availableSkills.Length);

        Console.Clear();

        var selectedSkill = availableSkills[input];
        return new UseSkillCommand(player, mob, selectedSkill);
    }


    private Skill[] LoadAvailableSkills()
    {
        var skills = this.player.Class?.SkillList.Where(x => x.requiredLevel <= this.player.LevelManager.CurrentLevel).Select(x => x.Skill).ToArray();
        if (skills == null || skills.Length == 0)
        {
            throw new InvalidOperationException("No skills available.");
        }

        return skills;
    }

    private CombatOutcome GetCombatOutcome()
    {
        if (this.player.CurrentHealth <= 0)
        {
            return CombatOutcome.Defeat;
        }
        else if (this.mob.CurrentHealth <= 0)
        {
            return CombatOutcome.Victory;
        }
        else if (this.mob.CurrentHealth > 0 && this.player.CurrentHealth <= 0)
        {
            return CombatOutcome.Flee;
        }

        return CombatOutcome.Flee;
    }

    private TextPacket PrintMobName(MobBase mob, CharacterBase player)
    {
        return (mob.Level - player.LevelManager.CurrentLevel) switch
        {
            <= -2 => new TextPacket(mob.Name, GameTextPrinter.GetColor(TextKind.EasyMob)),
            > -2 and <= 2 => new TextPacket(mob.Name, GameTextPrinter.GetColor(TextKind.Mob)),
            _ => new TextPacket(mob.Name, GameTextPrinter.GetColor(TextKind.HardMob)),
        };
    }
}