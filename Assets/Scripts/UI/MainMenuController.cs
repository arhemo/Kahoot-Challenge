using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public void Play() { SceneLoader.Load("KahootSelector"); }

    public void OpenHighscores()
    {
        GameSession.LeaderboardKahootId = null;
        SceneLoader.Load("Highscores");
    }

    public void OpenCreator() { SceneLoader.Load("KahootCreator"); }
    public void OpenReports() { SceneLoader.Load("ReportsList"); }
    public void OpenAbout() { SceneLoader.Load("About"); }

    public void Exit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
