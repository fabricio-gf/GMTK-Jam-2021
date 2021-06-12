using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    [SerializeField] private float delay = 1f;

    private void Awake()
    {
        Destroy(gameObject, delay);        
    }
}
