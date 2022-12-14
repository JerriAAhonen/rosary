using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Util;

public class TimeManager : Singleton<TimeManager>
{
	[Header("General")]
	[SerializeField] private float turnDuration;
	[SerializeField] private float transitionDuration;
	
	[Header("References")]
	[SerializeField] private Light directionalLight;
	[SerializeField] private Volume dayVolume;
	[SerializeField] private Volume nightVolume;
	
	[Header("Day")]
	[SerializeField] private float lightIntensity_DAY;
	[SerializeField] private Color lightColor_DAY;
	[SerializeField] private Material skybox_DAY;
	[SerializeField] private AudioEvent roosterSFX;

	[Header("Night")]
	[SerializeField] private float lightIntensity_NIGHT;
	[SerializeField] private Color lightColor_NIGHT;
	[SerializeField] private Material skybox_NIGHT;
	[SerializeField] private AudioEvent laughSFX;

	private bool day;
	private float timeUntilChange;
	private bool firstDay = true;
	private bool firstNight = true;
	
	public int TimeUntilChange => Mathf.FloorToInt(timeUntilChange);
	public bool Day { get; private set; }

	private void Start()
	{
		WorldManager.I.StartGame += OnGameStart;
		WorldManager.I.GameOver += OnGameOver;
		UISunMoonDisplay.I.Rotate(day);
		
		SetWithoutFading();
		AudioManager.I.PlayMusic(true);
	}

	private void Update()
	{
		if (!WorldManager.I.GameOn)
			return;
		
		timeUntilChange -= Time.deltaTime;

		if (timeUntilChange <= 0)
		{
			// Change roles
			ChangeDay();
			timeUntilChange = turnDuration;
		}
	}

	private void OnGameOver()
	{
		timeUntilChange = 0;
	}

	private void OnGameStart()
	{
	}

	private void ChangeDay()
	{
		day = !day;
		StartCoroutine(Routine());
		UISunMoonDisplay.I.Rotate(day);
	}

	private IEnumerator Routine()
	{
		var skyboxChanged = false;
		var elapsed = 0f;
		while (elapsed < transitionDuration)
		{
			var step = elapsed / transitionDuration;
			if (day)
			{
				directionalLight.intensity = Mathf.Lerp(lightIntensity_NIGHT, lightIntensity_DAY, step);
				dayVolume.weight = Mathf.Lerp(0, 1, step);
				nightVolume.weight = Mathf.Lerp(1, 0, step);
				RenderSettings.ambientSkyColor = Color.Lerp(lightColor_NIGHT, lightColor_DAY, step);
			}
			else
			{
				directionalLight.intensity = Mathf.Lerp(lightIntensity_DAY, lightIntensity_NIGHT, step);
				dayVolume.weight = Mathf.Lerp(1, 0, step);
				nightVolume.weight = Mathf.Lerp(0, 1, step);
				RenderSettings.ambientSkyColor = Color.Lerp(lightColor_DAY, lightColor_NIGHT, step);
			}
			
			if (elapsed > transitionDuration / 2f && !skyboxChanged)
			{
				UIDimmer.I.Blink(1f, () =>
				{
					// Change everything here!
					RenderSettings.skybox = day ? skybox_DAY : skybox_NIGHT;
					AudioManager.I.PlayOnce(day ? roosterSFX : laughSFX);
					Day = day;

					if (!day && firstNight)
					{
						UIBanner.I.Show("You are being hunted");
						firstNight = false;
					}

					if (firstDay)
					{
						UIBanner.I.Show("Catch the ghosts!");
						firstDay = false;
					}
					else
						AudioManager.I.PlayMusic(day);
				});
				skyboxChanged = true;
			}
			
			elapsed += Time.deltaTime;
			yield return null;
		}

		SetWithoutFading();
	}

	private void SetWithoutFading()
	{
		if (day)
		{
			directionalLight.intensity = lightIntensity_DAY;
			dayVolume.weight = 1;
			nightVolume.weight = 0;
			RenderSettings.ambientSkyColor = lightColor_DAY;
		}
		else
		{
			directionalLight.intensity = lightIntensity_NIGHT;
			dayVolume.weight = 0;
			nightVolume.weight = 1;
			RenderSettings.ambientSkyColor = lightColor_NIGHT;
		}
	}
}