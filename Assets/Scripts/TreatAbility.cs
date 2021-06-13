using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Inventory))]
public class TreatAbility : MonoBehaviour
{
    [SerializeField] private GameObject prefab;

    private Inventory _inventory = null;

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    public void OnTreat(InputValue input)
    {
        if (input.isPressed)
        {
            UseTreat();
        }
    }

    private void UseTreat()
    {
        if (RoundManager.instance.isPaused) return;
        
        if (_inventory.hasItem(PickupType.Treat))
        {
            Instantiate(prefab, transform);
            _inventory.UseItem(PickupType.Treat);
        }
    }
}
