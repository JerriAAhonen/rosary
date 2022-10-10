using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator netAnim;
	[SerializeField] private float swingAnimDuration;
	[SerializeField] private Collider netCollider;
	
	private MouseLook mouseLook;
	private PlayerMovement movement;
	private static readonly int Swing = Animator.StringToHash("Swing");

	private void Awake()
	{
		movement = GetComponent<PlayerMovement>();
		movement.Init();

		mouseLook = GetComponentInChildren<MouseLook>();
		mouseLook.Init(movement);
	}

	private void Start()
	{
		InputManager.I.Shoot += OnShoot;
		netCollider.enabled = false;
	}

	// Could use Animation Events, but they were weird and didn't have time to perehtyä
	private bool swinging;
	private float elapsedSwingTime;
	private void Update()
	{
		if (swinging)
		{
			elapsedSwingTime += Time.deltaTime;
			if (elapsedSwingTime >= swingAnimDuration)
			{
				swinging = false;
				elapsedSwingTime = 0;
				netCollider.enabled = false;
			}
		}

		if (Input.GetKeyDown(KeyCode.F3))
			godMode = !godMode;
	}

	private bool godMode;
	
	private void OnShoot()
	{
		if (!WorldManager.I.GameOn) return;
		
		netAnim.SetTrigger(Swing);
		netCollider.enabled = true;
		swinging = true;
	}

	public void Kill()
	{
		if (godMode)
			return;
		
		Debug.Log("Player died!");
		WorldManager.I.OnGameOver();
	}
}