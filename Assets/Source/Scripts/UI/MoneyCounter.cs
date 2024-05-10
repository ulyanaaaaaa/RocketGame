using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextTranslator))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class MoneyCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    private TextTranslator _textTranslator;
    private Rocket _rocket;
    private string _id = "money";
    
    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    private void Awake()
    {
        _counter = GetComponent<TextMeshProUGUI>();
        _textTranslator = GetComponent<TextTranslator>();
    }
    
    private void Start()
    {
        _textTranslator.TranslateText += CurrentMoney;
    }

    public void CurrentMoney()
    {
        _counter.text = _textTranslator.Translate(_id) + _rocket.Wallet.ToString();
    }
}
