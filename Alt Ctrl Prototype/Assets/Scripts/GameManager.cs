using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int Score { get { return Instance._score; } set { Instance._score = value; } }

    private int _score = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Debug.LogError("Multiple GameManager Instances, duplicate: " + name);
            Destroy(this);
            return;
        }

        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
