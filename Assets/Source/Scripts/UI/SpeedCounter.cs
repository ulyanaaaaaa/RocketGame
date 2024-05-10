using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextTranslator))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class SpeedCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    private TextTranslator _textTranslator;
    private Rocket _rocket;
    private string _id = "speed";

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
        _textTranslator.TranslateText += CurrentSpeed;
    }

    public void CurrentSpeed()
    {
        _counter.text = _textTranslator.Translate(_id) + _rocket.Speed.ToString();
    }
}
