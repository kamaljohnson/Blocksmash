using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreScript : MonoBehaviour {

    Text ScoreText;
    private void Start()
    {
        ScoreText = GetComponent<Text>();
    }
    private void Update()
    {
        ScoreText.text = GameBoardScript.score.ToString();
    }
}
