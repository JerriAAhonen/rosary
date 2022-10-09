using System;
using TMPro;
using UnityEngine;
using Util;

public class UI : Singleton<UI>
{
	[SerializeField] private GameObject core;
	[SerializeField] private GameObject meta;
	
	[SerializeField] private TextMeshProUGUI scoreLabel;

	private void Start()
	{
		core.SetActive(false);
		meta.SetActive(true);

		WorldManager.I.GameOver += OnGameOver;
	}

	public void OnStart()
	{
		core.SetActive(true);
		meta.SetActive(false);

		SetScore(0);
		
		// Start game
		WorldManager.I.OnStart();
	}

	private void OnGameOver()
	{
		core.SetActive(false);
		meta.SetActive(true);
	}

	public void SetScore(int score)
	{
		scoreLabel.text = $"Ghosts captured: {score}";
	}
}