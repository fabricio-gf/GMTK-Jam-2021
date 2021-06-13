using System;
using System.Collections.Generic;
using UnityEngine;

class Inventory : MonoBehaviour
{
    [Header("Item Quantities")]
    [SerializeField] private uint maxTreat = 3;
    [SerializeField] private uint maxStick = 1;
    [SerializeField] private uint initTreat = 3;
    [SerializeField] private uint initStick = 0;

    [Header("Items SO")]
    [SerializeField] private IntVariable stickQuantity;
    [SerializeField] private IntVariable treatQuantity;

    private Dictionary<PickupType, int> _maxPickupQuantity = new Dictionary<PickupType, int>();

    private void Start()
    {
        _maxPickupQuantity[PickupType.Treat] = (int)maxTreat;
        _maxPickupQuantity[PickupType.Stick] = (int)maxStick;

        treatQuantity.Value = (int)initTreat;
        stickQuantity.Value = (int)initStick;
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