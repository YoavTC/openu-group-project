using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedrunManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> checkpointObjects = new List<GameObject>();
    
    public void Start()
    {
        foreach (var cpObject in checkpointObjects)
        {
            Destroy(cpObject);
        }
    }
}
