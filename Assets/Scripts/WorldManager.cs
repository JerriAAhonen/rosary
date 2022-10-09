using System;
using UnityEngine;
using Util;

public class WorldManager : Singleton<WorldManager>
{
	[SerializeField] private Player player;
	[SerializeField] private bool hunt;

	public Player Player => player;
	public bool Hunt => hunt;

	public event Action StartGame;
	public event Action GameOver;

	public void OnStart()
	{
		StartGame?.Invoke();
		MapGenerator.I.Generate();
	}

	public void OnGameOver()
	{
		GameOver?.Invoke();
		MapGenerator.I.ClearLevel();
	}
}