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

	public bool GameOn { get; private set; }

	protected override void Awake()
	{
		base.Awake();
		
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
	}

	private void Update()
	{
		hunt = !TimeManager.I.Day;
	}

	public void OnStart()
	{
		StartGame?.Invoke();
		MapGenerator.I.Generate();
		
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		GameOn = true;
	}

	public void OnGameOver()
	{
		GameOver?.Invoke();
		MapGenerator.I.ClearLevel();
		
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;

		GameOn = false;

		player.transform.position = Vector3.zero;
	}
}