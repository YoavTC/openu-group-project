using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel : MonoBehaviour
{
    [SerializeField] private bool isDebugPanelEnabled;
    
    [SerializeField] private Transform debugVisual;
    [SerializeField] private float refreshRate;
    
    private DebugInformation[] debugInformations;
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        debugInformations = playerMovement.GetDebugInformation();

        PopulateDebugPanel();

        if (refreshRate == 0) refreshRate = 0.1f;
        InvokeRepeating(nameof(UpdateDebug), 0f, refreshRate);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            isDebugPanelEnabled = !isDebugPanelEnabled;
            GetComponent<RectTransform>().localScale = isDebugPanelEnabled ? Vector3.one : Vector3.zero;
        }
    }

    private void UpdateDebug()
    {
        if (isDebugPanelEnabled) PopulateDebugPanel();
    }

    private void PopulateDebugPanel()
    {
        HelperFunctions.DestroyChildren(transform);
        debugInformations = playerMovement.GetDebugInformation();
        foreach (DebugInformation debugEntry in debugInformations)
        {
            Transform newVisual = Instantiate(debugVisual, transform);
            if (debugEntry.value is Boolean)
            {
                newVisual.GetComponent<Image>().color = (bool) debugEntry.value ? Color.green : Color.red;
                newVisual.GetChild(0).GetComponent<TMP_Text>().text = debugEntry.name;
            }
            else if (debugEntry.value is float || debugEntry.value is int || debugEntry.value is long || debugEntry.value is double)
            {
                newVisual.GetComponent<Image>().color = Color.gray;
                newVisual.GetChild(0).GetComponent<TMP_Text>().text = debugEntry.name + ": " + debugEntry.value;
            }
        }
    }
}
