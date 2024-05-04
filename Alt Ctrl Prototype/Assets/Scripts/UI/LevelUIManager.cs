using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUIManager : MonoBehaviour
{
    public TMP_Text TXT_Points;

    // Update is called once per frame
    void Update()
    {
        TXT_Points.text = $"Points: {GameManager.Score}";
    }
}
