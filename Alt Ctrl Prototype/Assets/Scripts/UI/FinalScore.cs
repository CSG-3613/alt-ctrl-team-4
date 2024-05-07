using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinalScore : MonoBehaviour
{
    public TMP_Text TXT_Score;

    void Start()
    {
        TXT_Score.text = $"Score: {GameManager.Score}";
        GameManager.Score = 0;
    }
}
