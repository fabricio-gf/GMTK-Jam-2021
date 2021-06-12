using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugPosition : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one);
    }
}
