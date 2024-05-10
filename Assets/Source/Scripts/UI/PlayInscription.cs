using DG.Tweening;
using UnityEngine;

public class PlayInscription : MonoBehaviour
{
    private void Start()
    {
        transform.DOScale(2f, 1f / 2f).SetLoops(-1, LoopType.Yoyo);
        transform.DOScale(0.8f, 1f / 2f).SetLoops(-1, LoopType.Yoyo);
    }
}
