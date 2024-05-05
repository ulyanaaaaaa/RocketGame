using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    
    private void Awake()
    {
        _counter = GetComponent<TextMeshProUGUI>();
    }

    public void CurrentMoney(int count)
    {
        _counter.text = $"Money: {count.ToString()}";
    }
}
