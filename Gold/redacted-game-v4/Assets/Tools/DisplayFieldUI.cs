using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class DisplayFieldUI : MonoBehaviour
{
    [SerializeField] private bool update;
    [ShowIf("update")]
    [SerializeField] private float updateRate;
    [SerializeField] private StringFieldScriptableObject field;
    [TextArea] [SerializeField] private string prefix;

    private TMP_Text text;

    public void Check()
    {
        string memberID = PlayerPrefs.GetString("PlayerID");
        text = GetComponent<TMP_Text>();
        text.text = prefix + memberID;
        // if (field != null)
        // {
        //     text = GetComponent<TMP_Text>();
        //     Check();
        //
        //     if (update)
        //     {
        //         InvokeRepeating(nameof(Check), 0f, updateRate);
        //     }
        // }
    }

    // private void Check()
    // {
    //     text.text = prefix + field.stringField;
    // }
}
