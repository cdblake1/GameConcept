using Godot;
using System;

namespace GodotImpl;

#nullable enable

internal partial class ZoneManager : Node
{
		[Export]
		private PackedScene? _enemyScene;

		private SpawnSystem? _spawnSystem;
		private Node2D? _player;

		private EncounterData _encounterData = new()
		{
				SpawnInterval = 3f,
				MaxAlive = 5
		};

		private int spawnedCount;
		private int nextAvailableIdx;

		public Node2D?[]? SpawnedEntities;

		public override void _Ready()
		{
				System.Diagnostics.Debugger.Launch();
				SpawnedEntities = new Node2D[_encounterData.MaxAlive];
				_player = Utilities.FindPlayer(this);

				if (_enemyScene is null)
				{
						throw new InvalidOperationException("EnemyScene must be assigned in ZoneManager.");
				}

				_spawnSystem = new SpawnSystem(
						_enemyScene,
						_encounterData.SpawnInterval,
						_encounterData.MaxAlive,
						spawnRadiusValue: 100f);
		}

		public override void _Process(double delta)
		{
				if (_player is null)
				{
						throw new InvalidOperationException("Player not found in ZoneManager.");
				}

				if (SpawnedEntities is null)
				{
						throw new InvalidOperationException("SpawnedEntities array is not initialized in ZoneManager.");
				}

				if (spawnedCount <= _encounterData.MaxAlive)
				{
						var entity = _spawnSystem?.TrySpawnEntity(source: _player,
								spawnedCount: spawnedCount,
								timeDelta: delta);

						if (entity is not null)
						{
								SpawnedEntities[nextAvailableIdx] = entity;
								nextAvailableIdx = int.MaxValue;
								GD.Print($"Spawned entity at {entity.GlobalPosition}");

								GetTree().Root.AddChild(entity);
						}


						for (var i = 0; i < SpawnedEntities.Length; i++)
						{
								var spawn = SpawnedEntities[i];
								{
										var shouldDelete = false;
										if (spawn is not ICombatantInstance c)
										{
												shouldDelete = true;
										}
										else if (c.Combatant.CurrentHealth <= 0f)
										{
												shouldDelete = true;
												GameParts.Player.CurrentExperience += 10f;
										}

										if (shouldDelete)
										{
												spawn?.QueueFree();
												SpawnedEntities[i] = null;
												spawnedCount--;
										}

										if (i < nextAvailableIdx && SpawnedEntities[i] is null)
										{
												nextAvailableIdx = i;
										}
								}
						}
				}
		}
}

internal struct EncounterData
{
		public float SpawnInterval;
		public int MaxAlive;
}

