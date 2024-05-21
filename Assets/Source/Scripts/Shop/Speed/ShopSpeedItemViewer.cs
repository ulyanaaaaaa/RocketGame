using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ShopSpeedItem))]
[RequireComponent(typeof(TextTranslator))]
public class ShopSpeedItemViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _price;
    [SerializeField] private TextMeshProUGUI _description;
    [SerializeField] private Image _background;
    private ShopSpeedItem _item;
    private Rocket _rocket;
    private TextTranslator _textTranslator;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }

    private void Awake()
    {
        _textTranslator = GetComponent<TextTranslator>();
        _item = GetComponent<ShopSpeedItem>();
    }

    private void Start()
    {
        UpdateInfo();
        _textTranslator.TranslateText += UpdateInfo;
        _item.OnUpdate += UpdateInfo;
    }

    private void Update()
    {
        if (_item.Price > _rocket.Wallet)
        {
            _background.color = new Color(30 / 255f, 25 / 255f, 67 / 255f, 1f);
        }
        else
        {
            _background.color = new Color(72 / 255f, 55 / 255f, 192 / 255f, 1f);
        }
    }

    private void UpdateInfo()
    {
        _price.text = _textTranslator.Translate("price") + " " + _item.Price;
        _description.text = " ";
        _description.text = _textTranslator.Translate("add_speed") + " " + "+" +Math.Round(_item.Speed, 1);
    }
}
