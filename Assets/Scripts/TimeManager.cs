
using System;
using UnityEngine;
using UnityEngine.Rendering;
using Util;

public class TimeManager : Singleton<TimeManager>
{
	[SerializeField] private Light directionalLight;
	[SerializeField] private float lightIntensity_DAY;
	[SerializeField] private float lightIntensity_NIGHT;
	
	[SerializeField] private Volume dayVolume;
	[SerializeField] private Volume nightVolume;

	[SerializeField] private bool day;
	[SerializeField] private float turnDuration;

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