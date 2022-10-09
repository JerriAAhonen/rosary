using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Util;

public class InputManager : Singleton<InputManager>
{
	private const bool IsSprintToggle = true;
	private const bool IsCrouchToggle = false;

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

	public event Action Jump;
	public event Action<bool, bool> Crouch;
	public event Action<bool, bool> Sprint;
	public event Action Shoot;
	public event Action<bool> Aim;
	public event Action SwitchWeapon;
	public event Action ToggleDebugScreen;

	private void EnableInput()
	{
		movementAction = pia.Player.Move;
		movementAction.Enable();

		lookAction = pia.Player.Look;
		lookAction.Enable();

		pia.Player.Jump.performed += OnJumpPerformed;
		pia.Player.Jump.Enable();

		pia.Player.Crouch.performed += CrouchPressed;
		pia.Player.Crouch.canceled += CrouchCancelled;
		pia.Player.Crouch.Enable();

		pia.Player.Sprint.performed += SprintPressed;
		pia.Player.Sprint.canceled += SprintCancelled;
		pia.Player.Sprint.Enable();

		pia.Player.Shoot.performed += OnShootPerformed;
		pia.Player.Shoot.Enable();

		pia.Player.Aim.performed += OnAimPerformed;
		pia.Player.Aim.canceled += OnAimCancelled;
		pia.Player.Aim.Enable();

		pia.Player.PrimaryWeapon.performed += OnPrimaryWeaponPerformed;
		pia.Player.PrimaryWeapon.Enable();

		pia.Player.SecondaryWeapon.performed += OnSecondaryWeaponPerformed;
		pia.Player.SecondaryWeapon.Enable();

		pia.Player.DebugScreen.performed += OnDebugScreenPerformed;
		pia.Player.DebugScreen.Enable();

		removeListeners = RemoveListeners;

		void OnJumpPerformed(InputAction.CallbackContext _)
		{
			Jump?.Invoke();
		}

		void CrouchPressed(InputAction.CallbackContext _)
		{
			Crouch?.Invoke(true, IsCrouchToggle);
		}

		void CrouchCancelled(InputAction.CallbackContext _)
		{
			Crouch?.Invoke(false, IsCrouchToggle);
		}

		void SprintPressed(InputAction.CallbackContext _)
		{
			Sprint?.Invoke(true, IsSprintToggle);
		}

		void SprintCancelled(InputAction.CallbackContext _)
		{
			Sprint?.Invoke(false, IsSprintToggle);
		}

		void OnShootPerformed(InputAction.CallbackContext _)
		{
			Shoot?.Invoke();
		}

		void OnAimPerformed(InputAction.CallbackContext _)
		{
			Aim?.Invoke(true);
		}

		void OnAimCancelled(InputAction.CallbackContext _)
		{
			Aim?.Invoke(false);
		}

		void OnPrimaryWeaponPerformed(InputAction.CallbackContext _)
		{
			SwitchWeapon?.Invoke();
		}

		void OnSecondaryWeaponPerformed(InputAction.CallbackContext _)
		{
			SwitchWeapon?.Invoke();
		}

		void OnDebugScreenPerformed(InputAction.CallbackContext _)
		{
			ToggleDebugScreen?.Invoke();
		}

		void RemoveListeners()
		{
			pia.Player.Jump.performed -= OnJumpPerformed;
			pia.Player.Crouch.performed -= CrouchPressed;
			pia.Player.Crouch.canceled -= CrouchCancelled;
			pia.Player.Sprint.performed -= SprintPressed;
			pia.Player.Sprint.canceled -= SprintCancelled;
			pia.Player.Shoot.performed -= OnShootPerformed;
			pia.Player.Aim.performed -= OnAimPerformed;
			pia.Player.Aim.canceled -= OnAimCancelled;
			pia.Player.PrimaryWeapon.performed -= OnPrimaryWeaponPerformed;
			pia.Player.SecondaryWeapon.performed -= OnSecondaryWeaponPerformed;
			pia.Player.DebugScreen.performed -= OnDebugScreenPerformed;
		}
	}

	private void DisableInput()
	{
		removeListeners?.Invoke();

		movementAction.Disable();
		lookAction.Disable();

		pia.Player.Jump.Disable();
		pia.Player.Crouch.Disable();
		pia.Player.Sprint.Disable();
		pia.Player.Shoot.Disable();
		pia.Player.Aim.Disable();
	}
}