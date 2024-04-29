using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMovement : MonoBehaviour
{
    private Transform fireRoot;

    // Start is called before the first frame update
    void Start()
    {
        fireRoot = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = fireRoot.position;
        pos.x += Time.deltaTime * 5;
        fireRoot.position = pos;
    }
}
