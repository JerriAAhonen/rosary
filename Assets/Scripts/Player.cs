using UnityEngine;

public class Player : MonoBehaviour
{
	public static string HighScoreKey = "HighScore";
	
	[SerializeField] private Animator netAnim;
	[SerializeField] private Net net;
	[SerializeField] private float swingAnimDuration;
	[SerializeField] private Collider netCollider;
	[SerializeField] private AudioEvent swingSFX;
	[SerializeField] private AudioEvent deathSFX;

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
	}

	private void Start()
	{
		InputManager.I.Shoot += OnShoot;
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
		if (godMode || !WorldManager.I.GameOn)
			return;
		
		Debug.Log("Player died!");
		PlayerPrefs.SetInt(HighScoreKey, net.Score);
		AudioManager.I.PlayOnce(deathSFX);
		WorldManager.I.OnGameOver();
	}

	private void OnShoot()
	{
		if (!WorldManager.I.GameOn) return;
		
		netAnim.SetTrigger(Swing);
		netCollider.enabled = true;
		swinging = true;

		AudioManager.I.PlayOnce(swingSFX);
	}
}