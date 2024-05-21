using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour, IPause
{
    public Action OnRightClicked;
    public Action OnLeftClicked;
    public Action OnPlay;
    public Action OnShoot;
    private Rocket _rocket;
    private PauseService _pauseService;
    private bool _isPlay;
    private bool _isPause;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }

    private void Awake()
    {
        _pauseService = GetComponent<PauseService>();
    }

    private void Start()
    {
        _pauseService.AddPause(this);
        _rocket.OnDie += Die;
    }

    private void Update()
    {
        if (_isPause)
            return;

        if (Input.GetKey(KeyCode.D))
            OnRightClicked?.Invoke();
        if (Input.GetKey(KeyCode.A))
            OnLeftClicked?.Invoke();

        if (Input.GetKeyDown(KeyCode.W) && !_isPlay)
        {
            _isPlay = true;
            OnPlay?.Invoke();
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space)) && _isPlay)
        {
            OnShoot?.Invoke();
        }

        foreach (Touch touch in Input.touches)
        {
            if (touch.tapCount == 2 && !_isPlay)
            {
                _isPlay = true;
                OnPlay?.Invoke();
            }

            if (touch.tapCount == 1 && _isPlay)
                OnShoot?.Invoke();
        }
    }



    public void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
    }

    private void Die()
    {
        _isPlay = false;
    }
}
