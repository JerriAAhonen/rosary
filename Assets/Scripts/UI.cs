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
		WorldManager.I.GameOver += Refresh;
		WorldManager.I.StartGame += Refresh;

		Refresh();
	}

	public void OnStart()
	{
		WorldManager.I.OnStart();
	}

	private void Update()
	{
		if (WorldManager.I.GameOn)
			timer.text = TimeManager.I.TimeUntilChange.ToString();
	}

	private void Refresh()
	{
		var gameOn = WorldManager.I.GameOn;
		core.SetActive(gameOn);
		meta.SetActive(!gameOn);
		
		if (WorldManager.I.GameOn)
		{
			SetScore(0);
		}
		else
		{
			var highscore = PlayerPrefs.HasKey(Net.HighScoreKey) ? PlayerPrefs.GetInt(Net.HighScoreKey) : 0;
			highScoreLabel.text = $"Highscore: {highscore}";
		}
	}

	public void SetScore(int score)
	{
		scoreLabel.text = $"{score}";
	}
}