using System;
using UnityEngine;

public class FloatVariable
{
    public Action OnValueChanged;

    public FloatVariable(float initialValue)
    {
        Value = initialValue;
    }

    private float _value;

    public float Value
    {
        get => _value;
        set
        {
            _value = value;
            OnValueChanged?.Invoke();
        }
    }
}