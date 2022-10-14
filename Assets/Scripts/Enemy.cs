using System;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public enum EnemyState { Wander, Chase, RunAway }

public class Enemy : MonoBehaviour
{
	[SerializeField] private GameObject normalModel;
	[SerializeField] private GameObject angryModel;
	[SerializeField] private GameObject capturedPS;
	[SerializeField] private GameObject changedPS;
	[SerializeField] private AudioEvent capturedSFX;
	
	private NavMeshAgent agent;
	private NavMeshSurface navMesh;
	private EnemyState state;

	public event Action<Enemy> Captured;
	
	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
		normalModel.SetActive(true);
		angryModel.SetActive(false);
	}

	private void UpdateModel(bool hunt)
	{
		if (hunting != hunt)
		{
			hunting = hunt;
			normalModel.SetActive(!hunt);
			angryModel.SetActive(hunt);
			var ps = Instantiate(changedPS);
			ps.transform.position = transform.position + Vector3.up * 0.65f;
		}
	}
	
	private bool hunting;
	private Vector3 destination;
	private Vector3 awayFromPlayer;
	
	private const float slowUpdateInterval = 2f;
	private float elapsed;
	private void Update()
	{
		var playerPos = WorldManager.I.Player.transform.position;
		var dist = (playerPos - transform.position).sqrMagnitude;

		if (WorldManager.I.Hunt)
		{
			if (state != EnemyState.Chase)
			{
				state = EnemyState.Chase;
				elapsed = 999;
				UpdateModel(true);
			}
		}
		else if (dist <= 30)
		{
			if (state != EnemyState.RunAway)
			{
				state = EnemyState.RunAway;
				elapsed = 999;
				UpdateModel(false);
			}
		}
		else
		{
			if (state != EnemyState.Wander)
			{
				state = EnemyState.Wander;
				elapsed = 999;
				UpdateModel(false);
			}
		}

		NavMeshHit hit;

		switch (state)
		{
			case EnemyState.Wander:
				if (elapsed > slowUpdateInterval)
				{
					var randomDirection = Random.insideUnitSphere * 28f;
					randomDirection += transform.position;
					NavMesh.SamplePosition(randomDirection, out hit, 28f, 1);
					destination = hit.position;
					agent.SetDestination(destination);
					
					elapsed = 0;
				}
				break;
			case EnemyState.Chase:
				if (dist < 10f)
				{
					destination = playerPos;
					agent.SetDestination(destination);
				}
				else if (elapsed > slowUpdateInterval)
				{
					destination = playerPos;
					agent.SetDestination(destination);
					elapsed = 0;
				}
				break;
			case EnemyState.RunAway:
				if (true)
				{
					awayFromPlayer = transform.position - playerPos;
					awayFromPlayer += transform.position;
					var found = NavMesh.SamplePosition(awayFromPlayer, out hit, 14f, 1);
					if (!found)
						destination = GetRandomPos();
					else
						destination = hit.position;
					agent.SetDestination(destination);

					elapsed = 0;
				}
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}

		elapsed += Time.deltaTime;
	}

	private void OnTriggerEnter(Collider other)
	{
		var player = other.gameObject.GetComponent<Player>();
		if (hunting && player)
		{
			player.Kill();
		}
	}

	public void Capture()
	{
		Captured?.Invoke(this);
		var ps = Instantiate(capturedPS);
		ps.transform.position = transform.position + Vector3.up * 0.65f;

		AudioManager.I.PlayOnce(capturedSFX, transform.position);
		
		Destroy(gameObject);
	}

	private Vector3 GetRandomPos()
	{
		var x = Random.Range(-14f, 14f);
		var z = Random.Range(-14f, 14f);
		return new Vector3(x, 0, z);
	}
}