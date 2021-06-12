using System.Collections.Generic;
using UnityEngine;

class Inventory : MonoBehaviour
{
    [Header("Item Quantities")]
    [SerializeField] private uint maxTreat = 3;
    [SerializeField] private uint maxStick = 1;
    [SerializeField] private uint initTreat = 3;
    [SerializeField] private uint initStick = 0;

    [Header("Prefabs")]
    [SerializeField] private GameObject treatGO;
    [SerializeField] private GameObject stickGO;

    private Dictionary<PickupType, uint> _pickupQuantity = new Dictionary<PickupType, uint>();
    private Dictionary<PickupType, uint> _maxPickupQuantity = new Dictionary<PickupType, uint>();

    private void Start()
    {
        _maxPickupQuantity[PickupType.Treat] = maxTreat;
        _maxPickupQuantity[PickupType.Stick] = maxStick;

        _pickupQuantity[PickupType.Treat] = initTreat;
        _pickupQuantity[PickupType.Stick] = initStick;
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

        Instantiate(treatGO, transform);

        _pickupQuantity[PickupType.Treat]--;
    }

    public void UseStick()
    {
        if (_pickupQuantity[PickupType.Stick] == 0)
            return;

        Instantiate(stickGO, transform).GetComponent<ThrowableStick>().BeginThrow();

        _pickupQuantity[PickupType.Stick]--;
    }
}