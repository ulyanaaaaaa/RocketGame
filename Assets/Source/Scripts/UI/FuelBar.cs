using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour, IPause
{
    [SerializeField] private Image _image;
    private Rocket _rocket;
    private PauseService _pauseService;
    private bool _isPause;

    public void Setup(Rocket rocket, PauseService pauseService)
    {
        _pauseService = pauseService;
        _rocket = rocket;
    }

    private void Start()
    {
        _pauseService.AddPause(this);
    }

    private void Update()
    {
        if (_isPause)
            return;
        
        if (_rocket.Fuel > 100)
        {
            _image.fillAmount = _rocket.Fuel / 200;
        }
        else
        {
            _image.fillAmount = _rocket.Fuel / 100;
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
}