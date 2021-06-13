using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private LayerMask killMask;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if((killMask & (int)Mathf.Pow(2, other.gameObject.layer)) != 0)
        {
            print("Collided with " + other.gameObject);
            Destroy(other.gameObject);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.rotation*GetComponent<BoxCollider>().size);
    }
}
