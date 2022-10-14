using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Util;

public class UISunMoonDisplay : Singleton<UISunMoonDisplay>
{
	public void Rotate(bool day)
	{
		LeanTween.rotateZ(gameObject, day ? 180 : 0, 2.5f)
			.setEase(LeanTweenType.easeInOutBack);
	}
}