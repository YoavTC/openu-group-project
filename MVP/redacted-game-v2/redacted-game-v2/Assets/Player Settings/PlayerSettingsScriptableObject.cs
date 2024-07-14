using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "PlayerSettings")]
public class PlayerSettingsScriptableObject : ScriptableObject
{
    public bool showTimer;

    public void ToggleShowTimer()
    {
        showTimer = !showTimer;
    }
}
