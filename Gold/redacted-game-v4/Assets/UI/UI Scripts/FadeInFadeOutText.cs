using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class FadeInFadeOutText : MonoBehaviour
{
    [SerializeField] private float fadeInDuration, fadeOutDuration, fadeOutDelayDuration;
    [SerializeField] private Vector2 scales;
    IEnumerator Start()
    {
        TMP_Text text = GetComponent<TMP_Text>();
        text.DOFade(0, 0);
        Color color = text.color;
        color.a = 0f;
        text.color = color;
        text.rectTransform.localScale *= scales.x;
        
        text.DOFade(1, fadeInDuration).SetUpdate(true);
        text.rectTransform.DOScale(new Vector2(scales.y, scales.y), fadeInDuration + fadeOutDelayDuration + fadeOutDuration).SetEase(Ease.OutSine).SetUpdate(true);
        yield return HelperFunctions.GetWait(fadeInDuration + fadeOutDelayDuration);
        text.DOFade(0, fadeOutDuration).SetUpdate(true);
        yield return HelperFunctions.GetWait(fadeOutDuration);
        Destroy(transform.gameObject);
    }
}
