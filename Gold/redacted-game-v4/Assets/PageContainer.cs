using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PageContainer : MonoBehaviour
{
    [SerializeField] private float exitSpeed;
    [SerializeField] private Ease exitType;
    [SerializeField] private Vector2 exitDirection;
    public void HideChildren()
    {
        for (int i = 1; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<ComicWindow>().SetUp();
        }
        
        gameObject.SetActive(false);
    }

    public void TransitionOut()
    {
        Vector2 target = (Vector2) transform.position + exitDirection;
        transform.DOMove(target, exitSpeed).SetEase(exitType).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
