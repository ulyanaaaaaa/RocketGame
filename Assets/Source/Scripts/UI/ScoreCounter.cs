using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(TextTranslator))]
public class ScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    private Rocket _rocket;
    private TextTranslator _textTranslator;
    private string _id = "height";

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    private void Awake()
    {
        _counter = GetComponent<TextMeshProUGUI>();
        _textTranslator = GetComponent<TextTranslator>();
    }
    
    private void Update()
    {
        _counter.text = _textTranslator.Translate(_id) + "\n" + _rocket.transform.position.y;
    }
}
