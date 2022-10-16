using TMPro;
using UnityEngine;
using Util;

public class UIBanner : Singleton<UIBanner>
{
	private CanvasGroup cg;
	private TextMeshProUGUI label;
	
	protected override void Awake()
	{
		base.Awake();
		cg = GetComponent<CanvasGroup>();
		cg.alpha = 0f;

		label = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void Show(string text)
	{
		label.text = text;
		
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
