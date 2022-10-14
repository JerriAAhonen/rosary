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
		
		EnableNetMeshRenderers(false);
	}

	private void OnGameStart()
	{
		EnableNetMeshRenderers(true);
	}
	
	private void OnGameOver()
	{
		PlayerPrefs.SetInt(HighScoreKey, score);
		score = 0;

		EnableNetMeshRenderers(false);
	}

	private void EnableNetMeshRenderers(bool enable)
	{
		foreach (var meshRenderer in meshRenderers)
			meshRenderer.enabled = enable;
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