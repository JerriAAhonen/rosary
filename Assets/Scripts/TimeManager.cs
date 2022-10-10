
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Util;

public class TimeManager : Singleton<TimeManager>
{
	[Header("General")]
	[SerializeField] private bool day;
	[SerializeField] private float turnDuration;
	[SerializeField] private float transitionDuration = 5f;
	
	[Header("References")]
	[SerializeField] private Light directionalLight;
	[SerializeField] private Volume dayVolume;
	[SerializeField] private Volume nightVolume;
	
	[Header("Day")]
	[SerializeField] private float lightIntensity_DAY;
	[SerializeField] private Color lightColor_DAY;
	
	[Header("Night")]
	[SerializeField] private float lightIntensity_NIGHT;
	[SerializeField] private Color lightColor_NIGHT;

	public int TimeUntilChange => Mathf.FloorToInt(timeUntilChange);
	public bool Day => day;

	private float timeUntilChange;
	private void Update()
	{
		timeUntilChange -= Time.deltaTime;

		if (timeUntilChange <= 0)
		{
			// Change roles
			ChangeDay();
			timeUntilChange = turnDuration;
		}
	}

	private void ChangeDay()
	{
		day = !day;
		StartCoroutine(Routine());
	}

	private IEnumerator Routine()
	{
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
			
			elapsed += Time.deltaTime;
			yield return null;
		}
	}

	private void OnValidate()
	{
		if (!directionalLight || !dayVolume || !nightVolume)
			return;
		
		if (day)
		{
			directionalLight.intensity = lightIntensity_DAY;
			dayVolume.weight = 1;
			nightVolume.weight = 0;
		}
		else
		{
			directionalLight.intensity = lightIntensity_NIGHT;
			dayVolume.weight = 0;
			nightVolume.weight = 1;
		}
	}
}