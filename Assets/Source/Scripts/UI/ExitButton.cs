using System;
using UnityEngine;

public class ExitButton : MonoBehaviour
{
    public Action OnClick;

    public void Click()
    {
        OnClick?.Invoke();
    }
}
