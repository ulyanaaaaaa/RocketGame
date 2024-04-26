using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopSpeedItem : MonoBehaviour
{
    public Action OnUpdate;
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    private Rocket _rocket;
    private Button _button;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    private void Awake()
    {
        Load();
        _button = GetComponent<Button>();
        _button.onClick.AddListener(TryBuy);
    }

    private void TryBuy()
    {
        if (_rocket.TrySpend(Price))
        {
            _rocket.AddSpeed(Speed);
            Price *= 2;
            Speed *= 1.1f;
            OnUpdate?.Invoke();
            Save();
        }
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("ShopSpeed"))
            Speed = PlayerPrefs.GetFloat("ShopSpeed", Speed);
        
        if (PlayerPrefs.HasKey("ShopSpeedPrice"))
            Price = PlayerPrefs.GetInt("ShopSpeedPrice", Price);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("ShopSpeed", Speed);
        PlayerPrefs.SetInt("ShopSpeedPrice", Price);
    }
}
