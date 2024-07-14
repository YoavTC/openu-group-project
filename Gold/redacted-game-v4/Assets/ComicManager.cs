using System;
using UnityEngine;

public class ComicManager : Singleton<ComicManager>
{
    public Transform[] pages;
    private int currentPageIndex = 0;
    private int currentWindowIndex = 0;

    private bool canNext;

    private void Start()
    {
        pages = HelperFunctions.GetChildren(transform).ToArray();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || canNext)
        {
            EnterNextWindow();
        }
    }

    void EnterNextWindow()
    {
        if (currentPageIndex < pages.Length)
        {
            Transform currentPage = pages[currentPageIndex];

            if (currentWindowIndex < currentPage.childCount)
            {
                canNext = false;
                currentPage.GetChild(currentWindowIndex).GetComponent<ComicWindow>().Enter();
                currentWindowIndex++;
            }
            else
            {
                currentWindowIndex = 0;
                currentPageIndex++;
                if (currentPageIndex < pages.Length)
                {
                    canNext = false;
                    currentPage.GetChild(currentWindowIndex).GetComponent<ComicWindow>().Enter();
                    currentWindowIndex++;
                }
            }
        }
    }

    public void AnimationCompleted()
    {
        canNext = true;
    }
}