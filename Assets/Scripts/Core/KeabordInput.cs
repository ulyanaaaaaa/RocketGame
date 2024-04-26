using System;
using UnityEngine;

public class KeabordInput : MonoBehaviour
{
    public Action OnRightClicked;
    public Action OnLeftClicked;
    public Action OnPlay;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            OnRightClicked?.Invoke();
        if (Input.GetKey(KeyCode.A))
            OnLeftClicked?.Invoke();
        if (Input.GetKeyDown(KeyCode.W))
            OnPlay?.Invoke();
    }
}
