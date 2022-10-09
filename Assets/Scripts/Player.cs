using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
	private MouseLook mouseLook;
	private PlayerMovement movement;

	private void Awake()
	{
		movement = GetComponent<PlayerMovement>();
		movement.Init();

		mouseLook = GetComponentInChildren<MouseLook>();
		mouseLook.Init(movement);

		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}
}