using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInformation
{
    public string name;
    public object value;

    public DebugInformation(string name, object value)
    {
        this.name = name;
        this.value = value;
    }
}
