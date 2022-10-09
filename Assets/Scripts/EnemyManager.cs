using System;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] private List<EnemySpawner> spawners;
	[SerializeField] private float spawnInterval;
	[SerializeField] private int maxConcurrentEnemies;
	
	private float elapsed;
	private readonly List<Enemy> enemies = new();

	private void Update()
	{
		elapsed += Time.deltaTime;
		if (elapsed > spawnInterval && ShouldSpawn())
		{
			// Spawn
			var newEnemy = spawners.Random().Spawn();
			newEnemy.Captured += EnemyCaptured;
			enemies.Add(newEnemy);
			elapsed = 0;
		}
	}

	private bool ShouldSpawn()
	{
		if (enemies.Count < maxConcurrentEnemies)
		{
			return true;
		}

		return false;
	}

	private void EnemyCaptured(Enemy enemy)
	{
		enemy.Captured -= EnemyCaptured;
		enemies.Remove(enemy);
	}
}