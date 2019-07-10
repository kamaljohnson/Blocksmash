/*
/*using GooglePlayGames;
using GooglePlayGames.BasicApi;#1#
using UnityEngine;
using UnityEngine.UI;

public class PlayGamesScript : MonoBehaviour
{
	public Text loginText;
	
	void Start ()
	{

	}

	public void SignIn()
	{
		Social.localUser.Authenticate((bool success) =>
		{
			if (success)
			{
				loginText.text = "logged in";
			}
			else
			{
				loginText.text = "error occured";
			}
		});
		if (PlayerPrefs.HasKey("highScore"))
		{
			/*AddScoreToLeaderboard(GPGSIds.leaderboard_highscore, PlayerPrefs.GetInt("highScore"));#1#
		}
		else
		{
			/*AddScoreToLeaderboard(GPGSIds.leaderboard_highscore, 0);
			#1#
			PlayerPrefs.SetInt("highScore", 0);
		}
	}

	#region Achievements

	public static void UnlockAchievment(string id)
	{
		Social.ReportProgress(id, 100, success => { });
	}

	public static void IncrimentAchievment(string id, int stepsToIncrement)
	{
/*
		PlayGamesPlatform.Instance.IncrementAchievement(id, stepsToIncrement, success => { });
#1#
	}

	public static void ShowAchivementsUI()
	{
		Social.ShowAchievementsUI();
	}
	#endregion

	#region Leaderboards

	public static void AddScoreToLeaderboard(string leaderboardId, long score)
	{
		Social.ReportScore(score, leaderboardId, success => { });
	}

	public static void ShowLeaderboardUI()
	{
		Social.ShowLeaderboardUI();
	}
	#endregion
}
*/
