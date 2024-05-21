using DG.Tweening;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextTranslator))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class PlayInscription : MonoBehaviour
{
    private TextMeshProUGUI _text;
    private TextTranslator _textTranslator;
    private string _id = "inscreption";
    
    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
        _textTranslator = GetComponent<TextTranslator>();
    }
    
    private void Start()
    {
        transform.DOScale(2f, 1f / 2f).SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(0.8f, 1f / 2f).SetLoops(-1, LoopType.Yoyo);
        
        _textTranslator.TranslateText += TextInscription;
    }
    
    private void TextInscription()
    {
        _text.text = _textTranslator.Translate(_id);
    }
    
}
