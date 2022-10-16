using System.Collections;
using TMPro;
using UnityEngine;
using Util;

public class UI : Singleton<UI>
{
	[SerializeField] private GameObject core;
	[SerializeField] private GameObject meta;
	
	[SerializeField] private TextMeshProUGUI scoreLabel;
	[SerializeField] private TextMeshProUGUI timer;
	[SerializeField] private TextMeshProUGUI highScoreLabel;

	private CanvasGroup coreCG;
	private CanvasGroup metaCG;
	private RectTransform coreRT;
	private RectTransform metaRT;

	protected override void Awake()
	{
		base.Awake();
		coreCG = core.GetComponent<CanvasGroup>();
		metaCG = meta.GetComponent<CanvasGroup>();
		coreRT = core.GetComponent<RectTransform>();
		metaRT = meta.GetComponent<RectTransform>();
	}

	private void Start()
	{
		WorldManager.I.StartGame += OnGameStart;
		WorldManager.I.GameOver += OnGameOver;
		
		SetHighScore();
		coreCG.alpha = 0f;
		coreRT.anchoredPosition = Vector3.up * 1000f;
		metaCG.alpha = 1f;
	}

	#region Unity Events

	public void OnStartClicked()
	{
		WorldManager.I.OnStart();
	}

	#endregion

	private void Update()
	{
		if (WorldManager.I.GameOn)
			timer.text = TimeManager.I.TimeUntilChange.ToString();
	}

	private void OnGameStart()
	{
		StartCoroutine(StartRoutine());
	}

	private IEnumerator StartRoutine()
	{
		SetScore(0);
		
		// Tween meta out
		LeanTween.value(gameObject, 1f, 0f, 0.5f)
			.setOnUpdate(v => metaCG.alpha = v);

		LeanTween.value(gameObject, 0, 1000f, 0.5f)
			.setOnUpdate(v => metaRT.anchoredPosition = Vector3.up * v)
			.setEase(LeanTweenType.easeOutCirc);

		yield return new WaitForSeconds(0.5f);
		
		// Tween core in
		LeanTween.value(gameObject, 0f, 1f, 0.5f)
			.setOnUpdate(v => coreCG.alpha = v);

		LeanTween.value(gameObject, 1000f, 0f, 0.5f)
			.setOnUpdate(v => coreRT.anchoredPosition = Vector3.up * v)
			.setEase(LeanTweenType.easeOutCirc);
	}

	private void OnGameOver()
	{
		StartCoroutine(EndRoutine());
	}

	private IEnumerator EndRoutine()
	{
		SetHighScore();
		
		// Tween core out
		LeanTween.value(gameObject, 1f, 0f, 0.5f)
			.setOnUpdate(v => coreCG.alpha = v);

		LeanTween.value(gameObject, 0f, 1000f, 0.5f)
			.setOnUpdate(v => coreRT.anchoredPosition = Vector3.up * v)
			.setEase(LeanTweenType.easeOutCirc);
		
		yield return new WaitForSeconds(0.5f);
		
		// Tween meta in
		LeanTween.value(gameObject, 0f, 1f, 0.5f)
			.setOnUpdate(v => metaCG.alpha = v);

		LeanTween.value(gameObject, 1000f, 0f, 0.5f)
			.setOnUpdate(v => metaRT.anchoredPosition = Vector3.up * v)
			.setEase(LeanTweenType.easeOutCirc);
	}

	private void SetHighScore()
	{
		var highscore = PlayerPrefs.GetInt(Player.HighScoreKey, 0);
		highScoreLabel.text = $"Highscore: {highscore}";
	}

	public void SetScore(int score)
	{
		scoreLabel.text = $"{score}";
		scoreLabel.transform.localScale = Vector3.zero;
		LeanTween.scale(scoreLabel.gameObject, Vector3.one, 0.3f).setEase(LeanTweenType.easeOutBack);
	}
}