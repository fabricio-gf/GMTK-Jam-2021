using UnityEngine;

public class ThrowableStick : MonoBehaviour
{
    public float Magnitude = 1000;

    [SerializeField] private Rigidbody m_rigidbody;
    [SerializeField] private GameObject m_distraction;
    [SerializeField] private Collider m_collider;

    private void Awake()
    {
        if(m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.isKinematic = true;
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
}
