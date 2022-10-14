using UnityEngine;

public class AutoRotate : MonoBehaviour
{
	[SerializeField] private bool x;
	[SerializeField] private bool y;
	[SerializeField] private bool z;
	[SerializeField] private float speed;
	
	private void Update()
	{
		if (x)
			transform.Rotate(Vector3.right, speed);
		if (y)
			transform.Rotate(Vector3.up, speed);
		if (z)
			transform.Rotate(Vector3.forward, speed);
	}
}