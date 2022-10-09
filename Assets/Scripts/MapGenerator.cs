using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class MapGenerator : Singleton<MapGenerator>
{
	[SerializeField] private List<GameObject> roomPrefabs;
	[SerializeField] private List<Vector3> roomPositions;
	[SerializeField] private List<Vector3> rotations;
	[SerializeField] private List<GameObject> rooms;

	public event Action MapGenerated;

	public void Generate()
	{
		StartCoroutine(BuildRoutine());
	}

	public void ClearLevel()
	{
		foreach (var room in rooms)
		{
			Destroy(room);
		}
		
		rooms.Clear();
	}

	private IEnumerator BuildRoutine()
	{
		foreach (var pos in roomPositions)
		{
			var room = Instantiate(roomPrefabs.Random());
			room.transform.position = pos;
			room.transform.rotation = Quaternion.Euler(rotations.Random());
			rooms.Add(room);
			yield return null;
		}

		MapGenerated?.Invoke();
	}
}