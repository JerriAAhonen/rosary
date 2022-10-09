using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Util;

public class UI : Singleton<UI>
{
	[SerializeField] private TextMeshProUGUI scoreLabel;

	public void SetScore(int score)
	{
		scoreLabel.text = $"Ghosts captured: {score}";
	}
}