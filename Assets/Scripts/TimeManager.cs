
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