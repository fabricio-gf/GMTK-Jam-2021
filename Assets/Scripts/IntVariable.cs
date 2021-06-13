using System;
using UnityEngine;

[CreateAssetMenu(fileName = "int", menuName = "Variables/int", order = 0)]
public class IntVariable : ScriptableObject
{
    [SerializeField] private int _value;
    
    public Action OnValueChange;
    public int Value 
    {
        get => _value;
        set
        {
            _value = value;
            //OnValueChange?.Invoke();
            RoundManager.instance.OnStickCountChange();
            RoundManager.instance.OnTreatCountChange();
        }
    }
}