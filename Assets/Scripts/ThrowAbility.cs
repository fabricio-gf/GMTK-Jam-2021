using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Inventory))]
public class ThrowAbility : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private float arrowLength = 1f;

    private bool _isThrowing = false;
    private Inventory _inventory = null;

    private Vector2 MouseDirection
    {
        get
        {
            Vector2 pos = Mouse.current.position.ReadValue();
            pos = new Vector2((pos.x / Screen.width) - 0.5f, (pos.y / Screen.height) - 0.5f);
            return pos;
        }
    }

    private void Awake()
    {
        _inventory = GetComponent<Inventory>();
    }

    public void OnThrow(InputValue input)
    {
        if (input.isPressed)
            BeginThrow();
        else
            InterruptThrow();
    }
    private void BeginThrow()
    {
        if (!_isThrowing && _inventory.hasItem(PickupType.Stick))
        {
            _isThrowing = true;
        }
    }

    private void InterruptThrow()
    {
        _isThrowing = false;
    }

    private void Update()
    {
        if (!_isThrowing)
        {
            lineRenderer.enabled = false;
            return;
        }
        
        lineRenderer.enabled = true;

        Vector2 direction2D = MouseDirection;
        Vector3 throwDirection = (new Vector3(direction2D.x, 0f, direction2D.y).normalized + Vector3.up).normalized;
        lineRenderer.SetPosition(1, throwDirection * arrowLength);

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            _isThrowing = false;

            // For some reason it is inverted
            
            throwDirection = new Vector3(-throwDirection.x, throwDirection.y, -throwDirection.z);
            Throw(throwDirection);

            _inventory.UseItem(PickupType.Stick);
        }
    }

    private void Throw(Vector3 direction)
    {
        ThrowableStick stick = Instantiate(prefab, transform).GetComponent<ThrowableStick>();
        stick.Throw(direction);
    }
}
