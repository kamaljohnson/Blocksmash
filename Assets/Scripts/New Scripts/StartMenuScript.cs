using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenuScript : MonoBehaviour {

    public GameObject highScoreText;
    public GameObject highScoreTextNumber;
    public GameObject PlayButton;

    Animator highScoreAnimation;
    Animator highScoreNumberAnimation;
    Animator PlayButtonAnimation;

    Text highScore;
    private void Awake()
    {
        highScoreAnimation = highScoreText.GetComponent<Animator>();
        highScoreNumberAnimation = highScoreTextNumber.GetComponent<Animator>();
        PlayButtonAnimation = PlayButton.GetComponent<Animator>();

        highScore = highScoreTextNumber.GetComponent<Text>();

        if (PlayerPrefs.HasKey("highScore"))
            highScore.text = PlayerPrefs.GetInt("highScore").ToString();
        else
            highScore.text = "0";

        highScoreAnimation.Play("StartMenuHighScoreAnimation", 1, 0f);
        highScoreNumberAnimation.Play("StartMenuHighScoreNumberAnimation", 1, 0f);
        PlayButtonAnimation.Play("TapToPlayAnimation", 1, 0f);
    }
}
