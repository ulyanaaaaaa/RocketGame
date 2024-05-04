using System;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public Action OnPause;
    private PauseService _pauseService;
    
    public void Setup(PauseService pauseService)
    {
        _pauseService = pauseService;
    }

    public void Click()
    {
        OnPause?.Invoke();
        _pauseService.Pause();
    }
}
