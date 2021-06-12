using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkForward : MonoBehaviour
{

    public float walkSpeed;
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * (walkSpeed * Time.deltaTime));
    }
}
