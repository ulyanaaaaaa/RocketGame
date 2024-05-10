using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextTranslator))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class FuelCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    private TextTranslator _textTranslator;
    private Rocket _rocket;
    private string _id = "max_fuel";
    
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
        _textTranslator.TranslateText += CurrentMaxFuel;
    }

    public void CurrentMaxFuel()
    {
        _counter.text = _textTranslator.Translate(_id) + _rocket.MaxFuel.ToString();
    }
}
