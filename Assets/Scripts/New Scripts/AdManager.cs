using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviour {

    public static bool reward = false;
    public static bool isShowing;
    
    public void Update()
    {
        if (Advertisement.isShowing)
            isShowing = true;
        if (Advertisement.IsReady("rewardedVideo"))
            isShowing = false;

        if (reward)
        {
            reward = false;
            isShowing = false;
            FindObjectOfType<GameManager>().ExtraLife();
        }
    }
    public static void ShowAd()
    {
        var options = new ShowOptions();
        options.resultCallback = HandleShowResult;

        Advertisement.Show("rewardedVideo", options);
    }

    static void HandleShowResult(ShowResult result)
    {
        if (result == ShowResult.Finished)
        {
            reward = true;
            if (!GameManager.admuteflag)
            {
               FindObjectOfType<AudioManager>().Mute();
            }
        }
        else if (result == ShowResult.Skipped)
        {
            reward = false;
            if (!GameManager.admuteflag)
            {
                FindObjectOfType<AudioManager>().Mute();
            }
        }
        else if (result == ShowResult.Failed)
        {
            reward = false;
            if (!GameManager.admuteflag)
            {
                FindObjectOfType<AudioManager>().Mute();
            }
        }
    }
}
