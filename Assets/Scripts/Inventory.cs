using System;
using System.Collections.Generic;
using UnityEngine;

class Inventory : MonoBehaviour
{
    [Header("Item Quantities")]
    [SerializeField] private int maxTreat = 3;
    [SerializeField] private int maxStick = 1;
    [SerializeField] private int initTreat = 3;
    [SerializeField] private int initStick = 0;

    [Header("Items SO")]
    [SerializeField] private IntVariable stickQuantity;
    [SerializeField] private IntVariable treatQuantity;

    private Dictionary<PickupType, int> _maxPickupQuantity = new Dictionary<PickupType, int>();

    private void Start()
    {
        _maxPickupQuantity[PickupType.Treat] = maxTreat;
        _maxPickupQuantity[PickupType.Stick] = maxStick;

        treatQuantity.Value = initTreat;
        stickQuantity.Value = initStick;
    }

    public bool Add(PickupType item)
    {
        switch (item)
        {
            case PickupType.Treat:
                if (treatQuantity.Value >= _maxPickupQuantity[PickupType.Treat])
                    return false;
                else
                {
                    treatQuantity.Value++;
                    return true;
                }
            case PickupType.Stick:
                if(stickQuantity.Value >= _maxPickupQuantity[PickupType.Stick])
                    return false;
                else
                {
                    stickQuantity.Value++;
                    return true;
                }
        }
        return false;
    }

    public bool hasItem(PickupType type) 
    {
        if (type == PickupType.Stick)
            return stickQuantity.Value > 0;
        else if (type == PickupType.Treat)
            return treatQuantity.Value > 0;
        return false;
    }

    public void UseItem(PickupType type)
    {
        if (type == PickupType.Stick)
        {
            if (stickQuantity.Value <= 0)
                return;
            stickQuantity.Value--;
        }
        else if (type == PickupType.Treat)
        {
            if (treatQuantity.Value <= 0)
                return;
            treatQuantity.Value--;
        }
    }
}