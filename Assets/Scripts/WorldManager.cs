using System;
using UnityEngine;
using Util;

public class WorldManager : Singleton<WorldManager>
{
	[SerializeField] private Player player;
	[SerializeField] private Transform playerMainMenuPos;
	[SerializeField] private PlayerMovement playerMovement;
	[SerializeField] private bool hunt;

	public Player Player => player;
	public bool Hunt => hunt;

	public event Action StartGame;
	public event Action DisableEnemies;
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
		GameOn = true;
		StartGame?.Invoke();
		MapGenerator.I.Generate();
		
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		MovePlayerIntoMaze();
	}

	public void OnGameOver()
	{
		GameOn = false;
		DisableEnemies?.Invoke();
		
		playerMovement.enabled = false;
		
		UIBanner.I.Show("Better luck next time");
		LeanTween.delayedCall(4.5f, EndGame);

		void EndGame()
		{
			GameOver?.Invoke();
			MapGenerator.I.ClearLevel();
		
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.Confined;
			
			MovePlayerToMainMenu();
		}
	}

	private void MovePlayerIntoMaze()
	{
		LeanTween.value(gameObject, player.transform.position.y, 0, 1f)
			.setOnUpdate(v => player.transform.position = new Vector3(0, v, 0))
			.setOnComplete(() => playerMovement.enabled = true)
			.setEase(LeanTweenType.easeOutQuad);
	}
	
	private void MovePlayerToMainMenu()
	{
		LeanTween.value(gameObject, player.transform.position.y, playerMainMenuPos.position.y, 1f)
			.setOnUpdate(v => player.transform.position = new Vector3(0, v, 0))
			.setOnComplete(() => player.transform.position = playerMainMenuPos.position)
			.setEase(LeanTweenType.easeOutQuad);
		LeanTween.rotateLocal(player.gameObject, playerMainMenuPos.rotation.eulerAngles, 1f);
	}
}