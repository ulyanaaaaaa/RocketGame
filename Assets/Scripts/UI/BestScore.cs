using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class BestScore : MonoBehaviour
{
    private string _id = "best_score";
    private float _bestScore;
    private TextMeshProUGUI _text;
    private Rocket _rocket;
    private ISaveService _saveService;

    public void Setup(Rocket rocket)
    {
        _rocket = rocket;
    }

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        _saveService = new SaveService();
        _rocket.OnDie += () => SetBestScore(_rocket.transform.position.y);

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
        _text.text = _text.text = "Best height: " + _bestScore;
    }
}

public class BestScoreSaveData
{
    public float BestScore;
}
