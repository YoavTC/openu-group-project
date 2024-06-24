using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Fields/String")]
public class StringFieldScriptableObject : ScriptableObject
{
    public string stringField;
}

[CreateAssetMenu(menuName = "Fields/Int")]
public class IntFieldScriptableObject : ScriptableObject
{
    public int intField;
}

[CreateAssetMenu(menuName = "Fields/Float")]
public class FloatFieldScriptableObject : ScriptableObject
{
    public float floatField;
}

[CreateAssetMenu(menuName = "Fields/Bool")]
public class BoolFieldScriptableObject : ScriptableObject
{
    public bool boolField;
}