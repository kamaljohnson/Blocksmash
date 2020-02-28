using System;
using System.Collections;
using System.Collections.Generic;
using Unity.UNetWeaver;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public enum TutorialState
    {
        HandLeft,
        HandRight,
        HandTop,
        HandSlideLeft,
        HandSlideRight,
        HandTopTap,
        Complete
    }

    public static TutorialState CurrentState;
    
    public static bool PlayerInTutorial = false;
    public static bool PauseBlock = false;

    public GameObject handLeft;
    public GameObject handRight;
    public GameObject handTop;
    public GameObject handTopSlideLeft;
    public GameObject handTopSlideRight;
    public GameObject handTopTap;

    public static bool CurrentTutorialTaskDone = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentTutorialTaskDone)
        {
            CurrentTutorialTaskDone = false;
            CurrentState += 1;
            ActivateTask();
        }
    }

    public  void StartTutorial()
    {
        PlayerInTutorial = true;
        CurrentState = TutorialState.HandLeft;
        CurrentTutorialTaskDone = false;
        ActivateTask();
    }

    public void ActivateTask()
    {
        switch (CurrentState)
        {
            case TutorialState.HandLeft:
                handLeft.gameObject.SetActive(true);
                break;
            case TutorialState.HandRight:
                handLeft.gameObject.SetActive(false);
                handRight.gameObject.SetActive(true);
                break;
            case TutorialState.HandTop:
                handRight.gameObject.SetActive(false);
                handTop.gameObject.SetActive(true);
                break;
            case TutorialState.HandSlideLeft:
                handTop.gameObject.SetActive(false);
                handTopSlideLeft.gameObject.SetActive(true);
                break;
            case TutorialState.HandSlideRight:
                handTopSlideLeft.gameObject.SetActive(false);
                handTopSlideRight.gameObject.SetActive(true);
                break;
            case TutorialState.HandTopTap:
                handTopSlideRight.gameObject.SetActive(false);
                handTopTap.gameObject.SetActive(true);
                break;
            case TutorialState.Complete:
                handTopTap.gameObject.SetActive(false);
                Debug.Log("CurrentState = Complete");
                PlayerPrefs.SetInt("firstPlay", 1);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
