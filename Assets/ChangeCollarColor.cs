using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCollarColor : MonoBehaviour
{
    public Color[] collarColors;
    
    // Start is called before the first frame update
    void Awake()
    {
        GetComponent<MeshRenderer>().material.color = collarColors[Random.Range(0, collarColors.Length)];
    }
}
