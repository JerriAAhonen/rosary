using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
	[SerializeField] private Enemy prefab;

	public Enemy Spawn()
	{
		var enemy = Instantiate(prefab);
		var agent = enemy.GetComponent<NavMeshAgent>();
		agent.enabled = false;
		enemy.transform.position = transform.position;
		agent.enabled = true;
		return enemy;
	}
}