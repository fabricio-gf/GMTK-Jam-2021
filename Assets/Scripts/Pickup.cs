using System;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private PickupType _pickupType;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Inventory>(out var inv))
        {
            PickupAction(inv);
        }
    }

    private void PickupAction(Inventory inv)
    {
        if(inv.Add(_pickupType))
            Destroy(gameObject);
    }
}
