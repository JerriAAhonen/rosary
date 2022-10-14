using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Util;

public class UIDimmer : Singleton<UIDimmer>
{
    private CanvasGroup cg;

    protected override void Awake()
    {
        base.Awake();
        cg = GetComponent<CanvasGroup>();
    }

    public void Blink(float dur, Action onDark)
    {
        StartCoroutine(Routine(dur, onDark));
    }

    private IEnumerator Routine(float dur, Action onDark)
    {
        var halfDur = dur / 2f;
        var elapsed = 0f;
        while (elapsed < halfDur)
        {
            var step = elapsed / halfDur;
            cg.alpha = Mathf.Lerp(0, 1, step);
            
            elapsed += Time.deltaTime;
            yield return null;
        }

        cg.alpha = 1f;
        onDark?.Invoke();
        
        elapsed = 0;
        while (elapsed < halfDur)
        {
            var step = elapsed / halfDur;
            cg.alpha = Mathf.Lerp(1, 0, step);
            
            elapsed += Time.deltaTime;
            yield return null;
        }
        
        cg.alpha = 0f;
    }
}
