using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{
    public Material[] possibleMaterials;
        
    // Start is called before the first frame update
    void Awake()
    {
        var mats = GetComponent<MeshRenderer>().materials;
        mats[0] = possibleMaterials[Random.Range(0, possibleMaterials.Length)];
        GetComponent<MeshRenderer>().materials = mats;
    }
}
