using TMPro;
using UnityEngine;

public class FuelCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    
    private void Awake()
    {
        _counter = GetComponent<TextMeshProUGUI>();
    }

    public void CurrentMaxFuel(float count)
    {
        _counter.text = $"Max fuel: {count.ToString()}";
    }
}
