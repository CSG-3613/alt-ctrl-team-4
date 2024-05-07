using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public static int Score { get { return Instance._score; } set { Instance._score = value; } }
    public static float Timer { get { return Instance._time; } set { Instance._time = value; } }

    private int _score = 0;
    private float _time = 60;

    private Scene currentScene;
    private string sceneName;

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
        currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
    }

    // Update is called once per frame
    void Update()
    {
        if(_time > 0 && sceneName == "Level")
        {
            _time -= Time.deltaTime;
        }
        else if(_time <= 0 && sceneName == "Level")
        {
            SceneManager.LoadScene("GameEnd", LoadSceneMode.Single);
        }
    }
}
