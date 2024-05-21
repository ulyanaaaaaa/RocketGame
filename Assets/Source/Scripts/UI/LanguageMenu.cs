using System;
using UnityEngine;

public class LanguageMenu : MonoBehaviour
{
    public Action OnExit;
    public Translator Translator;
    
    public void Setup(Translator translator)
    {
        Translator = translator;
    }

    public void RussianClick()
    {
        Translator.ChangeLanguage(Language.Russian);
    }
    
    public void EnglishClick()
    {
        Translator.ChangeLanguage(Language.English);
    }
    
    public void Exit()
    {
        OnExit?.Invoke();
    }
}
