using System;
using UnityEngine;
using TMPro;

public class ResultsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text correctText;
    [SerializeField] private TMP_Text incorrectText;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        if (GameSession.SelectedKahoot == null)
        {
            SceneLoader.Load("MainMenu");
            return;
        }

        int total = GameSession.CorrectAnswers + GameSession.IncorrectAnswers;
        int percent = total > 0 ? Mathf.RoundToInt(100f * GameSession.CorrectAnswers / total) : 0;

        usernameText.text = GameSession.CurrentUsername + "  -  " + GameSession.SelectedKahootId;
        scoreText.text = GameSession.FinalScore + " PUNTOS";
        correctText.text = "Respuestas correctas: " + GameSession.CorrectAnswers + "  (" + percent + "%)";
        incorrectText.text = "Respuestas incorrectas: " + GameSession.IncorrectAnswers;
        timeText.text = "Tiempo total: " + GameSession.TotalTime.ToString("F1") + " segundos";

        SaveResults();
    }

    private void SaveResults()
    {
        try
        {
            ScoreEntry entry = new ScoreEntry
            {
                username = GameSession.CurrentUsername,
                score = GameSession.FinalScore,
                totalTime = GameSession.TotalTime,
                correctAnswers = GameSession.CorrectAnswers,
                date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            };

            int total;
            int position = LeaderboardManager.AddResult(GameSession.SelectedKahootId, entry, out total);
            statusText.text = "¡Guardado!  Puesto #" + position + " de " + total;
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Saving Results", ex, "Could not save the player's result to XML.");
            statusText.text = "No se pudo guardar el resultado.";
        }
    }

    public void PlayAgain()
    {
        GameSession.ResetMatch();
        SceneLoader.Load("KahootGame");
    }

    public void BackToMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void OpenLeaderboard()
    {
        GameSession.LeaderboardKahootId = GameSession.SelectedKahootId;
        SceneLoader.Load("Highscores");
    }
}
