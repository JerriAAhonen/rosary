using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float gravity = -30;
	[SerializeField] private float walkSpeed = 5;
	[SerializeField] private float sprintSpeed = 7;
	[SerializeField] private float jumpHeight = 1.5f;
	private CharacterController cc;

	private InputManager inputManager;
	private Transform tm;

	//------------------------------
	// Input

	private bool Sprinting { get; set; }
	private bool Crouching { get; set; }

	#region MonoBehaviour

	private void Awake()
	{
		tm = transform;
		cc = GetComponent<CharacterController>();
	}

	public void Init()
	{
		inputManager = InputManager.I;

		//------------------------------
		// Input

		inputManager.Jump += OnJump;
		inputManager.Crouch += OnCrouch;
		inputManager.Sprint += OnSprint;
	}

	private Vector3 verticalVel;

	private void Update()
	{
		var horMovement = inputManager.MovementInput;
		var horVel = tm.right * horMovement.x + tm.forward * horMovement.y;
		var finalSpeed = Sprinting ? sprintSpeed : walkSpeed;
		cc.Move(horVel * (finalSpeed * Time.deltaTime));

		if (!cc.isGrounded)
			verticalVel.y += gravity * Time.deltaTime;
		else
			verticalVel.y = -0.1f;
		cc.Move(verticalVel * Time.deltaTime);
	}

	private void OnDisable()
	{
		inputManager.Jump -= OnJump;
		inputManager.Crouch -= OnCrouch;
		inputManager.Sprint -= OnSprint;
	}

	#endregion

	#region Input

	public void Rotate(float yaw)
	{
		tm.eulerAngles += new Vector3(0, yaw, 0);
	}

	private void OnJump()
	{
		verticalVel.y = Mathf.Sqrt(-2f * jumpHeight * gravity);
	}

	private void OnCrouch(bool pressed, bool isToggle)
	{
		if (!isToggle)
			Crouching = pressed;
		else if (pressed)
			Crouching = !Crouching;
	}

	private void OnSprint(bool pressed, bool isToggle)
	{
		if (!isToggle)
			Sprinting = pressed;
		else if (pressed)
			Sprinting = !Sprinting;
	}

	#endregion
}