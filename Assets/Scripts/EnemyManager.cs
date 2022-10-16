using System.Collections.Generic;
using UnityEngine;
using Util;

public class EnemyManager : MonoBehaviour
{
	[SerializeField] private float spawnInterval;
	[SerializeField] private int maxConcurrentEnemies;

	private bool initialised;
	private float elapsed;
	private List<EnemySpawner> spawners = new();
	private readonly List<Enemy> enemies = new();

	private void Start()
	{
		MapGenerator.I.MapGenerated += OnMapGenerated;
		WorldManager.I.DisableEnemies += OnDisableEnemies;
		WorldManager.I.GameOver += OnGameOver;
	}

	private void OnMapGenerated()
	{
		var spawnerGOS = GameObject.FindGameObjectsWithTag("EnemySpawner");
		foreach (var o in spawnerGOS)
		{
			spawners.Add(o.GetComponent<EnemySpawner>());
		}

		initialised = true;
	}

	private void OnDisableEnemies()
	{
		foreach (var enemy in enemies)
		{
			enemy.enabled = false;
		}
	}
	
	private void OnGameOver()
	{
		foreach (var enemy in enemies)
		{
			Destroy(enemy.gameObject);
		}
		
		enemies.Clear();
		spawners.Clear();
		initialised = false;
	}

	private void Update()
	{
		if (!initialised) return;
		
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

	private bool ShouldSpawn() => WorldManager.I.GameOn && enemies.Count < maxConcurrentEnemies;

	private void EnemyCaptured(Enemy enemy)
	{
		enemy.Captured -= EnemyCaptured;
		enemies.Remove(enemy);
	}
}