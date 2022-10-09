using UnityEngine;

public class Net : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		var enemy = other.gameObject.GetComponent<Enemy>();
		if (enemy)
		{
			enemy.Capture();
		}
	}
}