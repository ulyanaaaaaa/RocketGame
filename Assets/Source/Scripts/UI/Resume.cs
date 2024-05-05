using System;
using UnityEngine;

public class Resume : MonoBehaviour
{
    public Action OnResume;
    private PauseService _pauseService;
    
    public void Setup(PauseService pauseService)
    {
        _pauseService = pauseService;
    }

    public void Click()
    {
        OnResume?.Invoke();
        _pauseService.Resume();
    }
}
