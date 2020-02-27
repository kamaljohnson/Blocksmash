using Unity.UNetWeaver;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static bool IsPlayingFirstTime = true;
    
    #region UI_ICONS
    public Sprite Icone_AudioOn;
    public Sprite Icone_AudioOff;
    public Sprite Icone_LeaderBoard;
    public Sprite Icone_Pause;
    public Sprite Icone_Play;
    #endregion    
    public static bool isPlaying = true;
    public static bool isPaused;
    public static bool isGameOver;
    public static bool isInfo;

    public static bool admuteflag = false;
    public static bool shownAd = false;

    public GameObject StartMenuUI;
    public GameObject AudioButton1;
    public GameObject AudioButton2;
    public GameObject LeaderBoardButton;

    public GameObject GameUI;
    public GameObject PauseButton;

    public GameObject PauseUI;
    public GameObject GameOverUI;
    public GameObject LeaderBoardUI;
    public GameObject WatchAdForExtraLifeUI;
    public GameObject InfoUI;

    private bool firstTutorialshowing = false;

    #region
    public Text StartMenuHighScore;
    public Text GameOverHighScore;
    public Text GameOverScore;
    public Text GameOverScore1;
    
    public static bool mute;
    #endregion

    public static GameManager instance;

    private void Awake()
    {
        
        if (!PlayerPrefs.HasKey("firstPlay"))
        {
            Debug.Log("firstPlay");
        }
        else
        {
            //Play tutorial game
            //PlayerPrefs.SetInt("firstPlay", 0);
        }
        
        FindObjectOfType<GameBoard>().Reset();
        
        AudioButton1.GetComponent<Image>().sprite = Icone_AudioOn;
        AudioButton2.GetComponent<Image>().sprite = Icone_AudioOn;
        LeaderBoardButton.GetComponent<Image>().sprite = Icone_LeaderBoard;
        PauseButton.GetComponent<Image>().sprite = Icone_Pause;

        mute = false;
        shownAd = false;
        Screen.orientation = ScreenOrientation.Portrait;
        
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        StartMenu();
        DontDestroyOnLoad(gameObject);
    }
    public void Update()
    {
        if (isInfo && Input.GetKey(KeyCode.Escape))
        {
            InfoExit();
        }
    }
    public void Play()
    {

        //setting the current UI elements 
        StartMenuUI.SetActive(false);
        GameUI.SetActive(true);
        PauseButton.SetActive(true);
        GameBoard.BoardAnimation("StartMenuExit");
        
        
        //initialisation
        isPlaying = true;
        isGameOver = false;
        isPaused = false;
        BottomCollisinTrigger.reachedNoRotationZone = false;

        FindObjectOfType<GameBoard>().CreateBlockSet();
    }
    public void Continue()
    {

        //setting the current UI elements
        PauseUI.SetActive(false);
        GameUI.SetActive(true);
        PauseButton.SetActive(true);

        //initialisation
        isPlaying = true;
        isGameOver = false;
        isPaused = false;
    }
    public void Pause()
    {
        
        FindObjectOfType<GameBoard>().ResetInputControls();
        PauseUI.SetActive(true);
        PauseButton.SetActive(false);
        isPlaying = false;
        isGameOver = false;
        isPaused = true;
    }
    public void AdUI()
    {
        GameUI.SetActive(false);

        isPlaying = false;
        isPaused = true;
        GameOverScore1.text = GameBoard.score.ToString();
        WatchAdForExtraLifeUI.SetActive(true);
    }

    public void Info()
    {
        isInfo = true;
    }

    public void InfoExit()
    {
        if (firstTutorialshowing)
        {
            firstTutorialshowing = false;
            StartMenu();
        }
        InfoUI.SetActive(false);
        isInfo = false;
    }
    
    public void GameOver()
    {
        /*PlayGamesScript.AddScoreToLeaderboard(GPGSIds.leaderboard_highscore, PlayerPrefs.GetInt("highScore"));
*/

        //setting the current UI elements

        GameUI.SetActive(false);

        GameOverUI.SetActive(true);

        //initialisation
        isPlaying = false;
        isGameOver = true;
        isPaused = false;
        if (PlayerPrefs.HasKey("highScore"))
        {
            GameOverHighScore.text = PlayerPrefs.GetInt("highScore").ToString();
        }
        else
        {
            GameOverHighScore.text = "0";
        }
        GameOverScore.text = GameBoard.score.ToString();
    }
    public void StartMenu()
    {
        FindObjectOfType<GameBoard>().Reset();

        if(!PlayerPrefs.HasKey("TutorialShown"))
        {
            firstTutorialshowing = true;
            PlayerPrefs.SetString("TutorialShown", "done");
            InfoUI.SetActive(true);
            StartMenuUI.SetActive(false);
        }
        else
        {
            GameOverUI.SetActive(false);
            StartMenuUI.SetActive(true);
            GameBoard.BoardAnimation("StartMenuEntry");
        }
        
        shownAd = false;

        //initialisation
        isPlaying = false;
        isGameOver = false;
        isPaused = false;    
        if(PlayerPrefs.HasKey("highScore"))
        {
            StartMenuHighScore.text = PlayerPrefs.GetInt("highScore").ToString();
        }
        else
        {
            StartMenuHighScore.text = "0";
        }
    }
    public void LeaderBoard()
    {		

        //setting the current UI elements
        StartMenuUI.SetActive(false);

        //initialisation
        isPlaying = false;
        isGameOver = false;
        isPaused = false;
        /*PlayGamesScript.ShowLeaderboardUI();*/
    }
    public void Mute()
    {
        string muteText = "";
        if(mute)
        {
            AudioButton1.GetComponent<Image>().sprite = Icone_AudioOn;
            AudioButton2.GetComponent<Image>().sprite = Icone_AudioOn;
        }
        else
        {
            AudioButton1.GetComponent<Image>().sprite = Icone_AudioOff;
            AudioButton2.GetComponent<Image>().sprite = Icone_AudioOff;
        }
        FindObjectOfType<AudioManager>().Mute();
    }
    public void ShowAd()
    {
        AdManager.ShowAd();
        admuteflag = mute;
        if(!mute)
            FindObjectOfType<AudioManager>().Mute();
    }
    public void ExtraLife()
    {
        FindObjectOfType<GameBoard>().ExtraLife();
        isGameOver = false;
        WatchAdForExtraLifeUI.SetActive(false);
        Pause();
        shownAd = true;
    }
}
