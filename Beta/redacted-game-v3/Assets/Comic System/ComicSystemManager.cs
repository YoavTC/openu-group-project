using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using NaughtyAttributes;
using Udar.SceneManager;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ComicSystemManager : MonoBehaviour
{
    [SerializeField] private SceneField nextScene;
    
    private ComicWindow[] comicWindows;
    [SerializeField] [ReadOnly] private int windowIndex, pageIndex;
    
    [SerializeField] private float nextWindowCooldown;
    private float elapsedTime;

    private void Start()
    {
        //Make children invisible
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        
        pageIndex = 0;
        SetWindows();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;
        
        if (Input.anyKeyDown)
        {
            if (elapsedTime > nextWindowCooldown)
            {
                elapsedTime = 0f;
                NextWindow();
            }
        }
    }

    private void SetWindows()
    {
        Transform pageParent = transform.GetChild(pageIndex);
        pageParent.gameObject.SetActive(true);
        comicWindows = HelperFunctions.GetChildren(pageParent).ToArray().Select(t => t.GetComponent<ComicWindow>()).Where(rb => rb != null).ToArray();
        windowIndex = 0;
    }

    private void NextWindow()
    {
        //Move to next page
        if (windowIndex == comicWindows.Length)
        {
            //All pages & windows read, move to next scene
            if (pageIndex == transform.childCount - 1)
            {
                SceneManager.LoadScene(nextScene.BuildIndex);
                return;
            }
            HelperFunctions.DestroyChildren(transform.GetChild(pageIndex));
            pageIndex++;
            SetWindows();
        }
        
        if (windowIndex > 0) comicWindows[windowIndex - 1].transform.DOKill(true);
        
        comicWindows[windowIndex].Enter();
        windowIndex++;
        //Play sound effect?
    }
}
