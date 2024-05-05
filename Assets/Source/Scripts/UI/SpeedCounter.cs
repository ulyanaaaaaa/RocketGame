using TMPro;
using UnityEngine;

public class SpeedCounter : MonoBehaviour
{
    private TextMeshProUGUI _counter;
    
    private void Awake()
    {
        _counter = GetComponent<TextMeshProUGUI>();
    }

    public void CurrentSpeed(float count)
    {
        _counter.text = $"Speed: {count.ToString()}";
    }
}
