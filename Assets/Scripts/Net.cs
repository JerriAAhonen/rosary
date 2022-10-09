using UnityEngine;

public class Net : MonoBehaviour
{
	private int score;
	
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