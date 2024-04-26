using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopFuelItem : MonoBehaviour
{
    public Action OnUpdate;
    [field: SerializeField] public float Fuel { get; private set; }
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
            _rocket.AddFuel(Fuel);
            Price *= 2;
            Fuel *= 1.1f;
            OnUpdate?.Invoke();
            Save();
        }
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("ShopFuel"))
            Fuel = PlayerPrefs.GetFloat("ShopFuel", Fuel);
        
        if (PlayerPrefs.HasKey("ShopFuelPrice"))
            Price = PlayerPrefs.GetInt("ShopFuelPrice", Price);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("ShopFuel", Fuel);
        PlayerPrefs.SetInt("ShopFuelPrice", Price);
    }
}
