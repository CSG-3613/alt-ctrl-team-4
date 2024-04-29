using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObjectMovement : MonoBehaviour
{
    private Transform root;

    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float verticalSpeed;

    private int inversion = 1;
    private float startHeight;

    // Start is called before the first frame update
    void Start()
    {
        root = this.transform;
        startHeight = root.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = root.position;
        pos.x += Time.deltaTime * horizontalSpeed;
        if(pos.y > startHeight + 12)
            inversion = -1;
        if (pos.y < startHeight)
            inversion = 1;
        pos.y += Time.deltaTime * inversion * verticalSpeed;
        root.position = pos;
    }
}
