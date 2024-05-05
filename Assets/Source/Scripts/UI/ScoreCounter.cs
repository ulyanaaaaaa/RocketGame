using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ScoreCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    private Rocket _rocket;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }
    
    private void Awake()
    {
        _counter = GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        _counter.text = "Height: \n" + _rocket.transform.position.y;
    }
}
