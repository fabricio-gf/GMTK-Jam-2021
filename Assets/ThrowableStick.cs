using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowableStick : MonoBehaviour
{
    public float Magnitude = 1000;

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private GameObject m_distraction;
    [SerializeField] private Collider m_collider;

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
        if(m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody>();
    }

    public void BeginThrow()
    {
        m_rigidbody.isKinematic = true;
        StartCoroutine(ThrowAction());
    }

    public void Throw(Vector3 direction)
    {
        m_rigidbody.isKinematic = false;
        m_rigidbody.useGravity = true;
        transform.parent = null;

        m_rigidbody.AddForce(direction * Magnitude, ForceMode.Impulse);

        m_collider.enabled = true;
        m_distraction.SetActive(true);
    }

    private IEnumerator ThrowAction()
    {
        yield return new WaitUntil( () => Mouse.current.leftButton.wasPressedThisFrame );

        Vector2 direction2D = MouseDirection;
        Vector3 throwDirection = new Vector3(direction2D.x, 0f, direction2D.y).normalized + Vector3.up;

        Throw(throwDirection.normalized);
    }
}
