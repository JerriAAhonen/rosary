using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIDebugField : MonoBehaviour
{
    [SerializeField] private string text;

    private TextMeshProUGUI label;
    
    private void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    public void SetInt(int value)
    {
        label.text = text + value;
    }

    public void SetBool(bool value)
    {
        label.text = text + value;
    }
}
