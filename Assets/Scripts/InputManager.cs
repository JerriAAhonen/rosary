using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class InputManager : Singleton<InputManager>
{
	public bool IsReady;
	private InputAction lookAction;
	private InputAction movementAction;

	private PlayerInputActions pia;
	private Action removeListeners;
	public Vector2 MovementInput { get; private set; }
	public Vector2 LookInput { get; private set; }

	private void Start()
	{
		pia ??= new PlayerInputActions();

		EnableInput();
		IsReady = true;
	}

	private void Update()
	{
		if (!IsReady) return;

		MovementInput = movementAction.ReadValue<Vector2>();
		LookInput = lookAction.ReadValue<Vector2>();
	}

	private void OnEnable()
	{
		if (IsReady)
			EnableInput();
	}

	private void OnDisable()
	{
		DisableInput();
	}
	
	public event Action Shoot;

	private void EnableInput()
	{
		movementAction = pia.Player.Move;
		movementAction.Enable();

		lookAction = pia.Player.Look;
		lookAction.Enable();

		pia.Player.Shoot.performed += OnShootPerformed;
		pia.Player.Shoot.Enable();
		
		removeListeners = RemoveListeners;

		void OnShootPerformed(InputAction.CallbackContext _) => Shoot?.Invoke();

		void RemoveListeners()
		{
			pia.Player.Shoot.performed -= OnShootPerformed;
		}
	}

	private void DisableInput()
	{
		removeListeners?.Invoke();

		movementAction.Disable();
		lookAction.Disable();

		pia.Player.Shoot.Disable();
	}
}