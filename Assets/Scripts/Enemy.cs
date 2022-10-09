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
	
	private NavMeshAgent agent;
	private NavMeshSurface navMesh;
	private EnemyState state;

	public event Action<Enemy> Captured;
	
	private void Awake()
	{
		agent = GetComponent<NavMeshAgent>();
	}

	private void UpdateModel(bool hunt)
	{
		if (hunting != hunt)
		{
			hunting = hunt;
			normalModel.SetActive(!hunt);
			angryModel.SetActive(hunt);
		}
	}
	
	private bool hunting;
	private Vector3 destination;
	
	private const float slowUpdateInterval = 2f;
	private const float RunawayInterval = 5f;
	private float elapsed;
	private void Update()
	{
		var playerPos = WorldManager.I.Player.transform.position;
		var dist = (playerPos - transform.position).sqrMagnitude;

		if (WorldManager.I.Hunt)
		{
			Debug.Log("State: Chase");
			state = EnemyState.Chase;
			UpdateModel(true);
		}
		else if (dist > 14)
		{
			Debug.Log("State: Wander");
			state = EnemyState.Wander;
			UpdateModel(false);
		}
		else
		{
			Debug.Log("State: Run Away");
			state = EnemyState.RunAway;
			UpdateModel(false);
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
				if (elapsed > RunawayInterval)
				{
					var awayFromPlayer = transform.position - playerPos * 14f;
					awayFromPlayer += transform.position;
					NavMesh.SamplePosition(awayFromPlayer, out hit, 28f, 1);
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

	private void OnDrawGizmos()
	{
		switch (state)
		{
			case EnemyState.Wander:
				Gizmos.color = Color.green;		
				break;
			case EnemyState.Chase:
				Gizmos.color = Color.red;
				break;
			case EnemyState.RunAway:
				Gizmos.color = Color.blue;
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
		
		Gizmos.DrawSphere(destination, 1f);
		Gizmos.DrawRay(transform.position + Vector3.up, ((destination + Vector3.up) - transform.position).normalized * 10);
	}
}