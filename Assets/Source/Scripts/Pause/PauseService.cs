using System.Collections.Generic;
using UnityEngine;

public class PauseService : MonoBehaviour
{
    private List<IPause> _pauses = new List<IPause>();
    
    public void AddPause(IPause pause)
    {
        _pauses.Add(pause);
    }

    public void RemovePause(IPause pause)
    {
        _pauses.Remove(pause);
    }
    
    public void Pause()
    {
        foreach (IPause pause in _pauses)
        {
            pause.Pause();
        }
    }

    public void Resume()
    {
        List<IPause> pausesCopy = new List<IPause>(_pauses);
        foreach (IPause pause in  pausesCopy)
        {
            pause.Resume();
        }
    }
}