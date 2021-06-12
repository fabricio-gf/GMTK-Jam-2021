using System.Collections.Generic;
using UnityEngine;

class Inventory : MonoBehaviour
{
    [SerializeField] private uint maxTreat = 3;
    [SerializeField] private uint maxStick = 1;

    private Dictionary<PickupType, uint> _pickupQuantity = new Dictionary<PickupType, uint>();
    private Dictionary<PickupType, uint> _maxPickupQuantity = new Dictionary<PickupType, uint>();

    private void Start()
    {
        _maxPickupQuantity[PickupType.Treat] = maxTreat;
        _maxPickupQuantity[PickupType.Stick] = maxStick;

        _pickupQuantity[PickupType.Treat] = 0;
        _pickupQuantity[PickupType.Stick] = 0;
    }

    public bool Add(PickupType item)
    {
        if(_pickupQuantity[item] == _maxPickupQuantity[item])
        {
            return false;
        }
        else
        {
            _pickupQuantity[item] = _pickupQuantity[item] + 1;
            return true;
        }
    }

    public void UseTreat()
    {
        if (_pickupQuantity[PickupType.Treat] == 0)
            return;

        // TODO: Call dogs to player

        _pickupQuantity[PickupType.Treat]--;
    }

    public void UseStick()
    {
        if (_pickupQuantity[PickupType.Stick] == 0)
            return;

        // TODO: Throw Stick at Mouse Direction or Initialize Throw

        _pickupQuantity[PickupType.Stick]--;
    }
}