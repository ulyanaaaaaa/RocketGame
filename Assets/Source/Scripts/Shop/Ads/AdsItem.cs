using UnityEngine;
using YG;

public class AdsItem : MonoBehaviour
{
    private Rocket _rocket;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += Rewarded;
    }

    private void Rewarded(int i)
    {
        _rocket.AddMoney(i);
    }
    
    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= Rewarded;
    }

    public void OpenAds()
    {
        YandexGame.RewVideoShow(10000);
    }
}
