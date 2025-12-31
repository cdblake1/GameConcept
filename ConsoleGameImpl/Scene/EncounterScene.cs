using GameData.src.Encounter;
using GameData.src.Mob;
using GameLogic.Player;
using Infrastructure.Json.Repositories.Initialize;
using System.Diagnostics.Metrics;
using static ConsoleGameImpl.CombatScene;

namespace ConsoleGameImpl.Scene;

public class EncounterScene
{
  private readonly GameTextPrinter textPrinter;
  private readonly EncounterSelector encounterSelector;

  public EncounterScene(GameTextPrinter textPrinter, EncounterSelector encounterSelector)
  {
    this.textPrinter = textPrinter ?? throw new ArgumentNullException(nameof(textPrinter));
    this.encounterSelector = encounterSelector ?? throw new ArgumentNullException(nameof(encounterSelector));
  }

  public void ShowScene(PlayerInstance player, bool bossEncounter)
  {
    List<EncounterDefinition> encounters = [this.encounterSelector.SelectEncounter(player.Level, bossEncounter) ?? throw new InvalidOperationException("Encounter does not exist")];

    var menu = new Menu("Encounters", [.. encounters.Select(e => new MenuOption(e.Presentation.Name))]);
    while (true)
    {
      var selectedOption = menu.ShowMenu();
      if (selectedOption == -1)
      {
        return; // Exit
      }

      if (selectedOption >= 0 && selectedOption < encounters.Count)
      {
        var selectedEncounter = encounters[selectedOption];
        StartEncounter(player, new(selectedEncounter));
        return;
      }
      else
      {
        textPrinter.Print("Invalid selection.");
        textPrinter.WaitForInput();
        continue;
      }
    }
  }

  public class EncounterInstance
  {
    private readonly EncounterDefinition definition;

    private bool isActive;
    private int currentDuration;

    public EncounterInstance(EncounterDefinition definition)
    {
      this.definition = definition;

      this.isActive = true;
      this.currentDuration = 0;
    }

    public bool IsActive => this.isActive;
    public int CurrentDuration => this.currentDuration;
    public EncounterDefinition Definition => this.definition;


    public readonly struct EncounterTurnSnapshot(MobDefinition mob)
    {
      public readonly MobDefinition Mob = mob;
    }

    public EncounterTurnSnapshot AdvanceTurn()
    {
      if (!this.IsActive)
      {
        throw new InvalidOperationException("Turn cannot be advanced. Encounter is not active");
      }

      this.currentDuration++;

      var mobWeights = this.definition.MobWeights;
      if (mobWeights == null || mobWeights.Length == 0)
      {
        throw new InvalidOperationException("No mobs defined for this encounter.");
      }

      int totalWeight = mobWeights.Sum(mw => mw.Weight);
      int roll = Random.Shared.Next(0, totalWeight);
      int cumulative = 0;
      EncounterMobWeight? selected = null;
      foreach (var mw in mobWeights)
      {
        cumulative += mw.Weight;
        if (roll < cumulative)
        {
          selected = mw;
          break;
        }
      }

      if (selected == null)
      {
        throw new InvalidOperationException("Failed to select a mob.");
      }

      // Use the repository to resolve MobId to MobDefinition
      var mob = Repositories.MobRepository.Get(selected.MobId);

      return new EncounterTurnSnapshot(mob);
    }
  }

  private void StartEncounter(PlayerInstance player, EncounterInstance encounter)
  {
    DialogueQueue.AddDialogue([
        $"You entered the {encounter.Definition.Presentation.Name}",
            ..encounter.Definition.Presentation.Description.Split('\n'),
        ]);

    while (encounter.IsActive)
    {
      DialogueQueue.AddDialogue([
          $"You have {encounter.Definition.Duration.Max - encounter.CurrentDuration} turns left.",
                $"You have {player.CurrentHealth}/{player.Stats.GetStatValue(GlobalStat.Health)} health remaining."
      ]);

      var input = new Menu("Encounter Actions", [
          new MenuOption("Continue Encounter"),
                new MenuOption("End Encounter")
      ]).ShowMenu();

      if (input == -1)
      {
        return; // Exit
      }

      if (input == 0)
      {
        var snapshot = encounter.AdvanceTurn();

        var outcome = new CombatScene(player, snapshot.Mob).StartScene();

        switch (outcome)
        {
          case CombatOutcome.Victory:
            break;
          case CombatOutcome.Defeat:
            encounter.EndEncounter();
            textPrinter.Print("You lost the encounter.");
            break;
          case CombatOutcome.Flee:
            encounter.EndEncounter();
            textPrinter.Print("You fled from the encounter.");
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
      else if (input == 1)
      {
        encounter.EndEncounter();
        textPrinter.Print("You ended the encounter early.");
        textPrinter.WaitForInput();
        break;
      }
    }

    if (!encounter.EncounterEndedEarly)
    {
      textPrinter.Print($"You have completed the {encounter.Name} encounter!");
      textPrinter.WaitForInput();

      var reward = encounter.EncounterReward();
      if (reward.GoldCoin is not null)
      {
        player.Gold += reward.GoldCoin;
        textPrinter.Print([
            new("You received"),
                    new($" {reward.GoldCoin.Amount}", GameTextPrinter.GetColor(TextKind.Gold)),
                    new(" Gold Coins.")
        ]);
        textPrinter.WaitForInput();

        foreach (var item in reward.Loot)
        {
          if (item is CraftingMaterial material)
          {
            player.Inventory.AddItem(material);
            textPrinter.Print([
                new("You received"),
                            new($" {material.Count}"),
                            new($" {material.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
                        ]);
            textPrinter.WaitForInput();
          }
          else if (item is Equipment equipment)
          {
            player.Inventory.AddItem(equipment);
            textPrinter.Print([
                new("You received"),
                            new($" {equipment.Name}", GameTextPrinter.GetColor(TextKind.LootRare)),
                            new("!")
            ]);
            textPrinter.WaitForInput();
          }
          else
          {
            player.Inventory.AddItem(item);
            textPrinter.Print([
                new("You received"),
                            new($" {item.Name}", GameTextPrinter.GetColor(TextKind.LootNormal)),
                            new("!")
            ]);
            textPrinter.WaitForInput();
            return;
          }
        }
      }
    }
  }


  private EncounterScene()
  {
    encounterSelector = EncounterSelector.Create();
    textPrinter = new GameTextPrinter();
  }

  public static EncounterScene Create()
  {
    return new EncounterScene();
  }
}
