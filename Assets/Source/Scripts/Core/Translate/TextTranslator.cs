using System;
using TMPro;
using UnityEngine;

public class TextTranslator : MonoBehaviour
{
    public Action TranslateText;
    
    [SerializeField] private Language _language;
    [SerializeField] private Translator _translator;
    [SerializeField] public string Id;
    [SerializeField] private bool _notTranslateOnStart;
  
    private TextMeshProUGUI _textMesh;
    private string _result;
    
    public void Setup(Translator translator)
    {
        _translator = translator;
    }

    private void Start()
    {
        if (_translator == null)
            throw new Exception("Translator is null");
            
        _language = _translator.Language;
        
        _translator.OnLanguageChanged += language =>
        {
            _language = language;
            
            if (!_notTranslateOnStart)
                Translate();
            
            TranslateText?.Invoke();
        };
        
        if (!_textMesh)
            _textMesh = GetComponentInChildren<TextMeshProUGUI>();
        
        if (!_textMesh)
            _textMesh = GetComponent<TextMeshProUGUI>();
        
        if (Id == "")
            return;
        
        if(_textMesh == null)
            return;
        
        if (!_notTranslateOnStart)
            Translate();
    }

    private void Translate()
    {
        TextAsset textAsset = Resources.Load<TextAsset>(AssetsPath.DictionaryPath.Dictionary);
        Debug.Log("transkate");
        string[] data = textAsset.text.Split(new char[] {'\n'});
        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] {'\t'});
            if (row.Length > 1 && !string.IsNullOrEmpty(row[0]) && row[0] == Id)
            {
                if ((int)_language + 1 < row.Length)
                    _result = row[(int)_language + 1];
                else
                    Debug.Log($"Translation not found for language: {_language}");
                break;
            }
        }

        if (string.IsNullOrEmpty(_result))
            Debug.Log($"Id or word not found: id: {Id}");

        _textMesh.text = _result;
    }

    
    public string Translate(string id)
    {
        if (_translator == null)
            throw new Exception("Translator is null");
        
        _language = _translator.Language;
        
        TextAsset textAsset = Resources.Load<TextAsset>(AssetsPath.DictionaryPath.Dictionary);
        string[] data = textAsset.text.Split(new char[] {'\n'});
        for (int i = 0; i < data.Length; i++)
        {
            string[] row = data[i].Split(new char[] {'\t'});
            if (row.Length > 1 && !string.IsNullOrEmpty(row[0]) && row[0] == id)
            {
                if ((int)_language + 1 < row.Length)
                    _result = row[(int)_language + 1];
                else
                    Debug.Log($"Translation not found for language: {_language}");
                break;
            }
        }

        if (string.IsNullOrEmpty(_result))
            Debug.Log($"Id or word not found: id: {id}");

        return _result;
    }
}

public enum Language
{
    Russian = 0,
    English = 1
}
