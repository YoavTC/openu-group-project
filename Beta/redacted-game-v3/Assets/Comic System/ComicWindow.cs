using System;
using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class ComicWindow : MonoBehaviour
{
    [Header("Window Components & Settings")]
    [SerializeField] private float entrySpeed;
    [SerializeField] private EntryType entryType;
    
    [EnableIf("entryType", EntryType.MOVE_IN)]
    [SerializeField] private Ease easeType;
    [EnableIf("entryType", EntryType.MOVE_IN)]
    [Space]
    [SerializeField] private Transform startingPosition;
    
    private Image image;
    private Vector3 endPosition;

    private void Start()
    {
        image = GetComponent<Image>();
        if (entryType == EntryType.FADE_IN)
        {
            //Set invisible
            image.DOFade(0, 0f);
        }
        if (entryType == EntryType.MOVE_IN)
        {
            endPosition = transform.position;
            transform.position = startingPosition.position;
        }
    }

    [Button]
    public void Enter()
    {
        if (entryType == EntryType.FADE_IN)
        {
            //Set invisible
            image.DOFade(1, entrySpeed).SetEase(easeType);
        }
        if (entryType == EntryType.MOVE_IN)
        {
            transform.DOMove(endPosition, entrySpeed).SetEase(easeType);
        }
    }
}

enum EntryType
{
    FADE_IN,
    MOVE_IN
}
