using UnityEngine;
using YG;

public class AdsButton : MonoBehaviour
{
    [SerializeField] private GameObject _failWindow;
    private Rocket _rocket;
    private PauseService _pauseService;

    public void Setup(Rocket rocket, PauseService pauseService)
    {
        _rocket = rocket;
        _pauseService = pauseService;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void Rewarded(int i)
    {
        _rocket.AddFuel(i);
        _rocket.Continue();
        _failWindow.SetActive(false);
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    public void OpenAds()
    {
        YandexGame.RewVideoShow(20);
    }
}
