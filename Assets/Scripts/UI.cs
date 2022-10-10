using System;
using TMPro;
using UnityEngine;
using Util;

public class UI : Singleton<UI>
{
	[SerializeField] private GameObject core;
	[SerializeField] private GameObject meta;
	
	[SerializeField] private TextMeshProUGUI scoreLabel;
	[SerializeField] private TextMeshProUGUI timer;
	[SerializeField] private TextMeshProUGUI highScoreLabel;

	private void Start()
	{
		core.SetActive(false);
		meta.SetActive(true);

		WorldManager.I.GameOver += OnGameOver;

		var highscore = PlayerPrefs.HasKey(Net.HighScoreKey) ? PlayerPrefs.GetInt(Net.HighScoreKey) : 0;
		highScoreLabel.text = $"Highscore: {highscore}";
	}

	public void OnStart()
	{
		core.SetActive(true);
		meta.SetActive(false);

		SetScore(0);
		
		// Start game
		WorldManager.I.OnStart();
	}

	private void Update()
	{
		timer.text = TimeManager.I.TimeUntilChange.ToString();
	}

	private void OnGameOver()
	{
		core.SetActive(false);
		meta.SetActive(true);
	}

	public void SetScore(int score)
	{
		scoreLabel.text = $"{score}";
	}
}