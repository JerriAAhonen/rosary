using System;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	private NavMeshAgent agent;

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
}