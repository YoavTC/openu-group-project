using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Fields/Int")]
public class IntFieldScriptableObject : ScriptableObject
{
    [SerializeField] private int _intField;
    public event Action<int> OnFieldChanged;
    
    public int intField
    {
        get { return _intField; }
        set
        {
            if (_intField != value)
            {
                _intField = value;
                OnFieldChanged?.Invoke(_intField);
            }
        }
    }
}