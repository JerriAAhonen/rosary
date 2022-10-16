using UnityEngine;
using Util;

public class UIHuntedInfo : Singleton<UIHuntedInfo>
{
	private CanvasGroup cg;
	
	protected override void Awake()
	{
		base.Awake();
		cg = GetComponent<CanvasGroup>();
		cg.alpha = 0f;
	}

	public void Show()
	{
		LeanTween.value(gameObject, 0f, 1f, 3f)
			.setOnUpdate(v => cg.alpha = v)
			.setOnComplete(() =>
			{
				LeanTween.delayedCall(1f, () =>
				{
					LeanTween.value(gameObject, 1f, 0f, 2f)
						.setOnUpdate(v => cg.alpha = v);
				});
			});
	}
}
