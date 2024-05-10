using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TranslateButton : MonoBehaviour
{
    public Action OnClick;

    public void Click()
    {
        OnClick?.Invoke();
    }
}

