using System;
using UnityEngine;

public class Translator : MonoBehaviour
{
    public Action<Language> OnLanguageChanged;
    private SaveService _saveService;
    private const string _id = "translator";
    
    [field:SerializeField] public Language Language { get; private set; }
    

    private void Start()
    {
        _saveService = new SaveService();
        
        if (!_saveService.Exists(_id))
        {
            Language = Language.English;
            Save();
        }
        else
        {
            Load();
        }
        
        ChangeLanguage((Language)Enum.Parse(typeof(Language), Language.ToString()));
    }

    public void ChangeLanguage(Language language)
    {
        Language = language;
        Debug.Log(Language.ToString());
        OnLanguageChanged?.Invoke(language);
        Save();
    }
    
    private void Save()
    {
        TranslatorSaveData e = new TranslatorSaveData();
        e.Language = Language;
        _saveService.Save(_id, e);
    }

    private void Load()
    {
        _saveService.Load<TranslatorSaveData>(_id, e =>
        {
            Language = e.Language;
        });
    }
}

public class TranslatorSaveData
{
    public Language Language;
}