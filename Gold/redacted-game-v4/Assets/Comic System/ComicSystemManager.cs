using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using NaughtyAttributes;
using Udar.SceneManager;
using UnityEngine.UI;

public class ComicSystemManager : Singleton<ComicSystemManager>
{
    [SerializeField] private int nextSceneIndex;
    
    private ComicWindow[] currentWindows;
    [SerializeField] [ReadOnly] private int windowIndex, pageIndex;
    [SerializeField] private bool canNextWindow;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<PageContainer>().HideChildren();
        }
        canNextWindow = true;
        pageIndex = -1;
        NextPage();
    }

    private void Update()
    {
        if (Input.anyKeyDown && canNextWindow)
        {
            StartCoroutine(NextWindow());
        }
    }

    private IEnumerator NextWindow()
    {
        canNextWindow = false;
        if (windowIndex == 1)
        {
            yield return HelperFunctions.GetWait(0.5f);
            InGameLogger.Log("delaying!", Color.cyan);
        }
        if (windowIndex >= currentWindows.Length)
        {
            NextPage();
        }
        else
        {
            currentWindows[windowIndex].Enter();
            windowIndex++;
        }
    }

    private void NextPage()
    {
        windowIndex = 1;
        pageIndex++;
        canNextWindow = true;

        if (pageIndex == transform.childCount)
        {
            Debug.Log("Move to new scene!");  
            SceneManager.LoadScene(nextSceneIndex);
            return;
        }
        if (pageIndex > 0)
        {
            //transform.GetChild(pageIndex - 1).gameObject.SetActive(false);
            transform.GetChild(pageIndex - 1).GetComponent<PageContainer>().TransitionOut();
        }
        transform.GetChild(pageIndex).gameObject.SetActive(true);
        currentWindows = HelperFunctions.GetChildren(transform.GetChild(pageIndex))
            .Select(i => i.GetComponent<ComicWindow>()).ToArray();
        StartCoroutine(NextWindow());
    }

    public void AnimationCompleted()
    {
        canNextWindow = true;
    }
}
