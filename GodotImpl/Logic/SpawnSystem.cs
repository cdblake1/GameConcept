using Godot;
using System;

namespace GodotImpl;

#nullable enable

public partial class SpawnSystem
{
		private readonly PackedScene _enemyScene;
		private readonly float _spawnInterval;
		private readonly int _maxAlive;
		private readonly float _spawnRadius;

		private float _timer;

		public SpawnSystem(
				PackedScene enemySceneReference,
				float spawnIntervalValue,
				int maxAliveValue,
				float spawnRadiusValue)
		{
				_enemyScene = enemySceneReference;
				_spawnInterval = spawnIntervalValue;
				_maxAlive = maxAliveValue;
				_spawnRadius = spawnRadiusValue;
		}

		public Node2D? TrySpawnEntity(Node2D source, int spawnedCount, double timeDelta)
		{
				if (_timer < _spawnInterval)
				{
						_timer += (float)timeDelta;
						return null;
				}

				if (spawnedCount >= _maxAlive)
				{
						return null;
				}


				_timer = 0f;
				return SpawnEntity(source);
		}

		private Node2D SpawnEntity(Node2D source)
		{
				var instance = _enemyScene.Instantiate<Node2D>();

				if (instance is null)
				{
						throw new InvalidOperationException("Failed to instantiate EnemyScene.");
				}

				float angle = Random.Shared.NextSingle() * Mathf.Tau;
				Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * _spawnRadius;
				Vector2 spawnPosition = source.GlobalPosition + offset;

				if (instance is Node2D node2D)
				{
						node2D.GlobalPosition = spawnPosition;
				}

				return instance;
		}
}
