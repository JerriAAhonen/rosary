using System;
using UnityEngine;

public class Net : MonoBehaviour
{
	public static string HighScoreKey = "HighScore";

	private MeshRenderer[] meshRenderers;
	private int score;

	private void Awake()
	{
		meshRenderers = GetComponentsInChildren<MeshRenderer>();
	}

	private void Start()
	{
		WorldManager.I.StartGame += OnGameStart;
		WorldManager.I.GameOver += OnGameOver;
	}

	private void OnGameStart()
	{
		foreach (var meshRenderer in meshRenderers)
			meshRenderer.enabled = true;
	}
	
	private void OnGameOver()
	{
		PlayerPrefs.SetInt(HighScoreKey, score);
		score = 0;
		
		foreach (var meshRenderer in meshRenderers)
			meshRenderer.enabled = false;
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