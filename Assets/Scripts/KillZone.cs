using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class KillZone : MonoBehaviour
{

    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        print("Collided with " + other.gameObject);
        Destroy(other.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, transform.rotation*GetComponent<BoxCollider>().size);
    }
}
