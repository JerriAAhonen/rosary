using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] private float walkSpeed = 5;
	
	private CharacterController cc;
	private InputManager inputManager;
	private Transform tm;

	#region MonoBehaviour

	private void Awake()
	{
		tm = transform;
		cc = GetComponent<CharacterController>();
	}

	public void Init()
	{
		inputManager = InputManager.I;
	}

	private void Update()
	{
		if (!WorldManager.I.GameOn)
			return;
		
		var horMovement = inputManager.MovementInput;
		var horVel = tm.right * horMovement.x + tm.forward * horMovement.y;
		cc.Move(horVel * (walkSpeed * Time.deltaTime));
	}

	#endregion

	#region Input

	public void Rotate(float yaw)
	{
		tm.eulerAngles += new Vector3(0, yaw, 0);
	}

	#endregion
}