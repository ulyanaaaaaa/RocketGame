using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
[RequireComponent(typeof(TextTranslator))]
public class BestScore : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private Rocket _rocket;
    private TextTranslator _textTranslator;
    private ISaveService _saveService;
    private string _id = "best_height";
    private float _bestScore;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _textTranslator = GetComponent<TextTranslator>();
    }

    private void Start()
    {
        _saveService = new SaveService();
        _rocket.OnDie += () => SetBestScore(_rocket.transform.position.y);
        _textTranslator.TranslateText += UpdateCounter;

        if (!_saveService.Exists(_id))
        {
            _bestScore = 0;
            Save();
        }
        else
            Load(); 
        
        UpdateCounter();
    }
    
    private void Save()
    {
        BestScoreSaveData data = new BestScoreSaveData();
        data.BestScore = _bestScore;
        _saveService.Save(_id, data);
        UpdateCounter();
    }

    private void Load()
    {
        _saveService.Load<BestScoreSaveData>(_id, data =>
        {
            _bestScore = data.BestScore;
        });
        UpdateCounter();
    }

    private void SetBestScore(float score)
    {
        if (score > _bestScore)
        {
            _bestScore = score;
            Save();
        }
    }

    private void UpdateCounter()
    {
        _text.text = _textTranslator.Translate(_id) + _bestScore;
    }
}

public class BestScoreSaveData
{
    public float BestScore;
}
