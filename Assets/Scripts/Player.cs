using UnityEngine;

public class Player : MonoBehaviour
{
	[SerializeField] private Animator netAnim;
	[SerializeField] private float swingAnimDuration;
	[SerializeField] private Collider netCollider;

	private GameObject net;
	private MouseLook mouseLook;
	private PlayerMovement movement;
	private bool godMode;
	
	private bool swinging;
	private float elapsedSwingTime;
	
	private static readonly int Swing = Animator.StringToHash("Swing");

	private void Awake()
	{
		movement = GetComponent<PlayerMovement>();
		movement.Init();

		mouseLook = GetComponentInChildren<MouseLook>();
		mouseLook.Init(movement);

		net = netCollider.gameObject;
	}

	private void Start()
	{
		InputManager.I.Shoot += OnShoot;
		WorldManager.I.StartGame += OnStartGame;
		netCollider.enabled = false;
	}
	
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
		{
			godMode = !godMode;
			Debug.Log($"God mode: {godMode}");
		}
	}
	
	public void Kill()
	{
		if (godMode)
			return;
		
		Debug.Log("Player died!");
		WorldManager.I.OnGameOver();
		net.SetActive(false);
	}

	private void OnShoot()
	{
		if (!WorldManager.I.GameOn) return;
		
		netAnim.SetTrigger(Swing);
		netCollider.enabled = true;
		swinging = true;
	}

	private void OnStartGame()
	{
		net.SetActive(true);
	}
}