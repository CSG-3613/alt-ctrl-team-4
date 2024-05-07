using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = System.Random;

public class ObjectMovement : MonoBehaviour
{
    private Transform root;
    private Vector3 pos;

    [SerializeField]
    private float horizontalSpeed;
    [SerializeField]
    private float verticalSpeed;

    private int inversion = 1; // Changes the y direction
    private float startHeight;
    private Random rand;

    // Start is called before the first frame update
    void Start()
    {
        root = this.transform;
        startHeight = root.position.y;
        rand = new Random();
    }

    // Update is called once per frame
    void Update()
    {
        pos = root.position;
        int randomNum = rand.Next(1,2);
        pos.x += Time.deltaTime * horizontalSpeed * randomNum;
        // Keeps object within bounds
        if(pos.y > startHeight + 12)
            inversion = -1;
        if (pos.y < startHeight)
            inversion = 1;
        pos.y += Time.deltaTime * inversion * verticalSpeed * randomNum;
        root.position = pos;
    }
}
