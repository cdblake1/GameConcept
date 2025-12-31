using GameLogic.Combat;
using GameLogic.Mob;
using GameLogic.Player;
using Infrastructure.Json.Repositories.Initialize;
using static GameLogic.Combat.CombatEngine;

namespace ConsoleGameImpl;

public class CombatScene(PlayerInstance player, MobInstance mob)
{
  private readonly PlayerInstance player = player ?? throw new ArgumentNullException(nameof(player));
  private readonly MobInstance mob = mob ?? throw new ArgumentNullException(nameof(mob));
  private readonly GameTextPrinter textPrinter = new();

  private readonly CombatEngine combatEngine = new(
    Repositories.SkillRepository,
    Repositories.EffectRepository,
    Repositories.TalentRepository,
    player,
    mob);

  private readonly Menu menu = new("Combat Menu",
  [
      new MenuOption("Attack", ConsoleKey.A),
        new MenuOption("Check Consumables", ConsoleKey.C),
        new MenuOption("Run"),
    ]);

  public bool StartScene()
  {
    do
    {
      var input = this.menu.ShowMenu();
      Console.Clear();

      switch (input)
      {
        case 0:
          var skillId = this.SelectSkill();
          if (skillId is null)
          {
            continue;
          }

          var command = new UseSkillCommand(skillId, this.mob.MobDefinition.Id);
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
          //RunAway();
          return false;
        default:
          break;
      }
    } while (this.combatEngine.IsCombatActive);

    return true;
  }

  //public CombatEvent StartSceneOld()
  //{
  //  EnterCombat();

  //  var outcome = GetCombatOutcome();

  //  if (CombatEvent.Victory == outcome)
  //  {
  //    var experienceReward = mob.AwardExperience();

  //    DialogueQueue.AddDialogue(
  //        [[
  //                  new TextPacket($"You gained"),
  //                  new TextPacket($" {experienceReward}", GameTextPrinter.GetColor(TextKind.Experience)),
  //                  new TextPacket(" experience points.")
  //              ]]);

  //    var leveled = this.player.LevelManager.AddExperience(experienceReward);

  //    if (leveled)
  //    {
  //      DialogueQueue.AddDialogue(
  //          [
  //              [new TextPacket("You leveled up!")],
  //                      [
  //                          new TextPacket($" You are now level "),
  //                          new TextPacket($"{this.player.LevelManager.CurrentLevel}", GameTextPrinter.GetColor(TextKind.LevelUp)),
  //                          new TextPacket("!")
  //                      ]
  //          ]);

  //      this.textPrinter.WaitForInput();
  //    }
  //    else
  //    {
  //      DialogueQueue.AddDialogue([[
  //                  new("You are "),
  //                  new($"{this.player.LevelManager.GetExperienceNeededForNextLevel() - this.player.LevelManager.CurrentExperience} ", GameTextPrinter.GetColor(TextKind.Experience)),
  //                  new("experience away from levelling.")
  //              ]]);
  //    }

  //    var loot = mob.DropLoot();
  //    if (loot != null)
  //    {
  //      player.Inventory.AddItem(loot);

  //      if (loot is CraftingMaterial material)
  //      {
  //        textPrinter.Print([
  //            new TextPacket("You received"),
  //                      new TextPacket($" {material.Count}"),
  //                      new TextPacket($" {material.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
  //                  ]);

  //        textPrinter.WaitForInput();
  //      }
  //      else if (loot is Equipment equipment)
  //      {
  //        textPrinter.Print([
  //            new TextPacket("You received "),
  //                      new TextPacket($" {equipment.Name}", GameTextPrinter.GetColor(TextKind.LootRare)),
  //                      new TextPacket("!")
  //        ]);

  //        textPrinter.WaitForInput();
  //      }
  //      else
  //      {
  //        textPrinter.Print([
  //            new TextPacket("You received "),
  //                      new TextPacket($" {loot.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
  //                      new TextPacket("!")
  //        ]);

  //        textPrinter.WaitForInput();
  //      }
  //    }
  //    else
  //    {
  //      textPrinter.Print("No loot dropped.");
  //      textPrinter.WaitForInput();
  //    }

  //  }
  //  else if (CombatOutcome.Defeat == outcome)
  //  {
  //    this.textPrinter.Print(
  //        [
  //            new TextPacket("You have lost!"),
  //                  new TextPacket("You have been defeated and will respawn at the last checkpoint.")
  //        ]);

  //    this.textPrinter.WaitForInput();
  //  }
  //  else if (CombatOutcome.Flee == outcome)
  //  {
  //    this.textPrinter.Print(
  //        [
  //            new TextPacket("You fled!")
  //        ]);

  //    this.textPrinter.WaitForInput();
  //  }

  //  return outcome;
  //}

  public string? SelectSkill()
  {
    var skills = this.player.GetSelectedSkills();

    var menu = new Menu("Select Skill",
      skills.Select(s => new MenuOption(s)).ToList());

    do
    {
      var input = menu.ShowMenu();

      if(input == -1)
      {
        break;
      }
      else if (input < 0 || input >= skills.Count)
      {
        GameTextPrinter.DefaultInstance.Print("Skill selection invalid");
        continue;
      }

      var skill = skills[input];
      return skill;

    } while (true);

    return null;
  }

  //private void PreCombat()
  //{
  //  foreach (var evt in this.combatManager.InitializeTurn())
  //  {
  //    RenderEvent(evt);
  //  }
  //}

  //private void RenderEvent(CombatEvent evt)
  //{
  //  switch (evt)
  //  {
  //    case CombatStartEvent startEvent:
  //      List<List<TextPacket>> startMessages = [];
  //      foreach (var mob in startEvent.Mobs)
  //      {
  //        startMessages.Add([
  //            new TextPacket($"A "),
  //                      PrintMobName(mob, startEvent.Players[0]),
  //                      new(" has appeared!")]);
  //      }

  //      startMessages.Add([new TextPacket("Prepare for combat!")]);

  //      DialogueQueue.AddDialogue([.. startMessages]);
  //      break;
  //    case CombatEndEvent endEvent:
  //      if (endEvent.playerWon)
  //      {
  //        DialogueQueue.AddDialogue([[new("You have defeated "), PrintMobName(mob, player), new("!")]]);
  //      }
  //      else
  //      {
  //        DialogueQueue.AddDialogue([[
  //                      new TextPacket("You have lost to "),
  //                      PrintMobName(mob, player),
  //                      new TextPacket("!")
  //                  ]]);
  //      }

  //      return;
  //    case DamageEvent damageEvent:
  //      if (damageEvent.Target is CharacterBase character)
  //      {
  //        DialogueQueue.AddDialogue([
  //            [
  //                          new TextPacket($"{character.Name} took "),
  //                          PrintDamage((int)damageEvent.Damage),
  //                          new TextPacket(" damage!")
  //                      ],
  //                      [
  //                          new TextPacket($"{character.Name} has "),
  //                          new($"{character.CurrentHealth}", GameTextPrinter.GetColor(TextKind.Health)),
  //                          new(" HP left.")
  //                      ]
  //        ]);
  //      }
  //      else if (damageEvent.Target is MobBase mob)
  //      {
  //        DialogueQueue.AddDialogue([
  //            [
  //                          PrintMobName(mob, this.player),
  //                          new TextPacket(" took "),
  //                          PrintDamage((int)damageEvent.Damage),
  //                          new TextPacket(" damage!")
  //                      ],
  //                      [
  //                          PrintMobName(mob, this.player),
  //                          new TextPacket(" has "),
  //                          new($"{mob.CurrentHealth}", GameTextPrinter.GetColor(TextKind.Health)),
  //                          new(" HP left.")
  //                      ]
  //        ]);
  //      }
  //      break;
  //    case StatusEffectEvent statusEffectEvent:
  //      if (statusEffectEvent.Target is CharacterBase chartwo)
  //      {
  //        this.textPrinter.Print(
  //            [
  //                new TextPacket($"{chartwo.Name} is affected by "),
  //                              PrintStatusName(statusEffectEvent.Effect),
  //                              new TextPacket(".")
  //            ]);

  //        this.textPrinter.WaitForInput();
  //      }
  //      else if (statusEffectEvent.Target is MobBase mob)
  //      {
  //        this.textPrinter.Print(
  //            [
  //                PrintMobName(mob, this.player),
  //                              new TextPacket(" is affected by "),
  //                              PrintStatusName(statusEffectEvent.Effect),
  //                              new TextPacket(".")
  //            ]);

  //        this.textPrinter.WaitForInput();
  //      }
  //      break;
  //    case FleeEvent fleeEvent:
  //      if (fleeEvent.success)
  //      {
  //        this.textPrinter.Print(
  //            [
  //                new TextPacket($"{fleeEvent.Actor.Name} successfully fled the combat!"),
  //                      ]);

  //        this.textPrinter.WaitForInput();
  //      }
  //      else
  //      {
  //        this.textPrinter.Print(
  //            [
  //                new TextPacket($"{fleeEvent.Actor.Name} failed to flee the combat!"),
  //                      ]);

  //        this.textPrinter.WaitForInput();
  //      }
  //      break;
  //    case ActorDefeatedEvent defeatedEvent:
  //      if (defeatedEvent.Actor is CharacterBase charThree)
  //      {
  //        this.textPrinter.Print(
  //            [
  //                new TextPacket($"{charThree.Name} has been defeated!"),
  //                      ]);

  //        this.textPrinter.WaitForInput();
  //      }
  //      else if (defeatedEvent.Actor is MobBase mob)
  //      {
  //        this.textPrinter.Print(
  //            [
  //                PrintMobName(mob, this.player),
  //                          new TextPacket(" has been defeated!"),
  //                      ]);

  //        this.textPrinter.WaitForInput();
  //      }
  //      break;
  //  }
  //}

  public enum CombatOutcome
  {
    Victory,
    Defeat,
    Flee
  }

  private TextPacket PrintDamage(int damage)
  {
    return new TextPacket($"{damage}", GameTextPrinter.GetColor(TextKind.Damage));
  }

  private TextPacket PrintMobName(MobInstance mob, PlayerInstance player)
  {
    return (mob.Level - player.LevelManager.CurrentLevel) switch
    {
      <= -2 => new TextPacket(mob.MobDefinition.Presentation.Name, GameTextPrinter.GetColor(TextKind.EasyMob)),
      > -2 and <= 2 => new TextPacket(mob.MobDefinition.Presentation.Name, GameTextPrinter.GetColor(TextKind.Mob)),
      _ => new TextPacket(mob.MobDefinition.Presentation.Name, GameTextPrinter.GetColor(TextKind.HardMob)),
    };
  }
}
