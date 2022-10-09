using System;
using UnityEngine;

public class Net : MonoBehaviour
{
	public static string HighScoreKey = "HighScore";
	
	private int score;

	private void Start()
	{
		WorldManager.I.GameOver += OnGameOver;
	}

	private void OnGameOver()
	{
		PlayerPrefs.SetInt(HighScoreKey, score);
		score = 0;
	}
	
	private void OnTriggerEnter(Collider other)
	{
		var enemy = other.gameObject.GetComponent<Enemy>();
		if (enemy)
		{
			enemy.Capture();
			score++;
			UI.I.SetScore(score);
		}
	}
}