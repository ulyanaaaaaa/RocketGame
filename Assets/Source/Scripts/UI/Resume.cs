using System;
using UnityEngine;

[RequireComponent(typeof(TextTranslator))]
public class Resume : MonoBehaviour
{
    public Action OnResume;
    private PauseService _pauseService;
    private string _id = "resume";
    
    public void Setup(PauseService pauseService)
    {
        _pauseService = pauseService;
    }
    
    private void Awake()
    {
        GetComponent<TextTranslator>().Id = _id;
    }
    
    public void Click()
    {
        OnResume?.Invoke();
        _pauseService.Resume();
    }
}
