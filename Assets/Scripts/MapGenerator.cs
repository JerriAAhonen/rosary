using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class MapGenerator : Singleton<MapGenerator>
{
	[SerializeField] private List<GameObject> rooms;
	[SerializeField] private List<Vector3> roomPositions;
	[SerializeField] private List<Vector3> rotations;

	public event Action MapGenerated;
	
	public void Start()
	{
		StartCoroutine(BuildRoutine());
	}

	private IEnumerator BuildRoutine()
	{
		foreach (var pos in roomPositions)
		{
			var room = Instantiate(rooms.Random());
			room.transform.position = pos;
			room.transform.rotation = Quaternion.Euler(rotations.Random());
			yield return null;
		}

		MapGenerated?.Invoke();
	}
}