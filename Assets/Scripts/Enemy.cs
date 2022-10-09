using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	private NavMeshAgent agent;

	public event Action<Enemy> Captured;
	
	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void Update()
	{
		var playerPos = WorldManager.I.Player.transform.position;
		agent.destination = playerPos;
	}

	private void OnTriggerEnter(Collider other)
	{
		var player = other.gameObject.GetComponent<Player>();
		if (player)
		{
			player.Kill();
		}
	}

	public void Capture()
	{
		Debug.Log("Enemy captured");
		Captured?.Invoke(this);
		Destroy(gameObject);
	}
}