using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OpenLinkedinProfile : MonoBehaviour
{
    public void OpenInbarURL() => Application.OpenURL("https://www.linkedin.com/in/inbar-gal-bb1125308/");
    public void OpenYoavURL() => Application.OpenURL("https://www.linkedin.com/in/yoav-trachtman-cohen/");
    public void OpenOhadURL() => Application.OpenURL("https://www.linkedin.com/in/ohad-dori/");
    public void OpenAlexURL() => Application.OpenURL("https://www.linkedin.com/in/alex-yakovlev-430124308/");

    private int gInput;
    private bool gMode = false;
    [SerializeField] private Image gingerUIElement;
    [SerializeField] private TMP_Text yoavDesc;
    
    private void Update()
    {
        if (!gMode && Input.GetKeyDown(KeyCode.G))
        {
            gInput++;
            if (gInput >= 20)
            {
                gMode = true;
                gingerUIElement.enabled = true;
                yoavDesc.text = "Ginger. My dog (:";
            }
        }
    }
}
