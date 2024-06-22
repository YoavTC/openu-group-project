using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class HideTempColliders : MonoBehaviour
{
    [Button]
    public void ToggleCollidersVisibility()
    {
        Transform[] children = HelperFunctions.GetChildren(transform).ToArray();

        for (int i = 0; i < children.Length; i++)
        {
            children[i].GetComponent<SpriteRenderer>().enabled = !children[i].GetComponent<SpriteRenderer>().enabled;
        }
    }
}
