using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ShopSpeedItem : MonoBehaviour
{
    public Action OnUpdate;
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    private string _id = "shop_speed_item";
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
            _rocket.AddSpeed(Speed);
            Price *= 2;
            Speed *= 1.1f;
            OnUpdate?.Invoke();
            Save();
        }
    }
    
    private void Save()
    {
        ShopSpeedItemSaveData data = new ShopSpeedItemSaveData();
        data.Speed = Speed;
        data.Price = Price;
        _saveService.Save(_id, data);
        OnUpdate?.Invoke();
    }

    private void Load()
    {
        _saveService.Load<ShopSpeedItemSaveData>(_id, data =>
        {
            Price = data.Price;
            Speed = data.Speed;
        });
        OnUpdate?.Invoke();
    }
}

public class ShopSpeedItemSaveData
{
    public float Speed;
    public int Price;
}
