using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ComicWindow : MonoBehaviour
{
    [Header("Window Components & Settings")]
    [SerializeField] private float entrySpeed;
    [SerializeField] private EntryType entryType;
    [SerializeField] private Ease easeType;
    
    private Transform offScreenPosition;
    private Vector3 onScreenPosition;
    private Image image;

    public void SetUp()
    {
        Debug.Log(gameObject + " Starting", gameObject);
        offScreenPosition = transform.GetChild(0);
        onScreenPosition = transform.position;
        transform.position = offScreenPosition.position;
        image = GetComponent<Image>();
        if (entryType == EntryType.FADE_IN) image.DOFade(0, 0f);
    }
    
    public void Enter()
    {
        Debug.Log(gameObject + " Entering", gameObject);
        if (entryType == EntryType.FADE_IN)
        {
            image.DOFade(1, entrySpeed).SetEase(easeType).OnComplete(() =>
            {
                ComicSystemManager.Instance.AnimationCompleted();
            });
        }
        if (entryType == EntryType.MOVE_IN)
        {
            transform.DOMove(onScreenPosition, entrySpeed).SetEase(easeType).OnComplete(() =>
            {
                ComicSystemManager.Instance.AnimationCompleted();
            });
        }
    }
}

enum EntryType
{
    FADE_IN,
    MOVE_IN
}
