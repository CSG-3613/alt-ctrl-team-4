using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterRestrict : MonoBehaviour
{
    private Transform root;
    private Vector3 pos;

    // Start is called before the first frame update
    void Start()
    {
        root = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        pos = root.position;
        pos.z = 0;
        root.position = pos;
    }
}
