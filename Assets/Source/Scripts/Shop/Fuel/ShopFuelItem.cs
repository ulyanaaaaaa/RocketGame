using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopFuelItem : MonoBehaviour
{
    public Action OnUpdate;
    [field: SerializeField] public float Fuel { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    private string _id = "shop_fuel_item";
    private Rocket _rocket;
    private Button _button;
    private ISaveService _saveService;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(TryBuy);
    }
    
    private void Start()
    {
        _saveService = new SaveService();
        
        if (!_saveService.Exists(_id))
        {
            Save();
        }
        else
            Load(); 
    }

    private void TryBuy()
    {
        if (_rocket.TrySpend(Price))
        {
            _rocket.AddFuel(Fuel);
            Price *= 10;
            Fuel *= 1.01f;
            OnUpdate?.Invoke();
            Save();
        }
    }

    private void Save()
    {
        ShopFuelItemSaveData data = new ShopFuelItemSaveData();
        data.Fuel = Fuel;
        data.Price = Price;
        _saveService.Save(_id, data);
        OnUpdate?.Invoke();
    }

    private void Load()
    {
        _saveService.Load<ShopFuelItemSaveData>(_id, data =>
        {
            Price = data.Price;
            Fuel = data.Fuel;
        });
        OnUpdate?.Invoke();
    }
}

public class ShopFuelItemSaveData
{
    public float Fuel;
    public int Price;
}
