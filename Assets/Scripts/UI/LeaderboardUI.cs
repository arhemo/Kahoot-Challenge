using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown kahootDropdown;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject entryPrefab;
    [SerializeField] private TMP_Text statusText;

    private List<string> kahootNames;

    private void Start()
    {
        try
        {
            kahootNames = KahootManager.GetAvailableKahoots();

            kahootDropdown.ClearOptions();
            kahootDropdown.AddOptions(kahootNames);
            kahootDropdown.onValueChanged.AddListener(ShowSelectedLeaderboard);

            int start = Mathf.Max(0, kahootNames.IndexOf(GameSession.LeaderboardKahootId));
            kahootDropdown.SetValueWithoutNotify(start);

            if (kahootNames.Count > 0)
                ShowSelectedLeaderboard(start);
            else
                statusText.text = "No hay Kahoots disponibles.";
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Loading Highscores", ex, "Could not load available questionnaires.");
            statusText.text = "No se pudieron cargar las clasificaciones.";
        }
    }

    private void ShowSelectedLeaderboard(int index)
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        try
        {
            // Si no existe el XML el ranking sale vacio.
            LeaderboardData data = LeaderboardManager.LoadLeaderboard(kahootNames[index]);
            statusText.text = data.entries.Count == 0 ? "Todavía no hay puntuaciones." : "";

            for (int i = 0; i < data.entries.Count; i++)
            {
                ScoreEntry entry = data.entries[i];
                GameObject row = Instantiate(entryPrefab, contentParent);

                row.transform.Find("PositionText").GetComponent<TMP_Text>().text = "#" + (i + 1);
                row.transform.Find("UsernameText").GetComponent<TMP_Text>().text = entry.username;
                row.transform.Find("ScoreText").GetComponent<TMP_Text>().text = entry.score.ToString();
                row.transform.Find("TimeText").GetComponent<TMP_Text>().text = entry.totalTime.ToString("F1") + "s";
                row.transform.Find("DateText").GetComponent<TMP_Text>().text = entry.date;

                // Podio: oro, plata y bronce.
                if (i < 3)
                {
                    Color[] podium = { new Color32(0xFF, 0xC8, 0x3D, 255), new Color32(0xC9, 0xD1, 0xE6, 255), new Color32(0xD9, 0x8B, 0x4E, 255) };
                    row.GetComponent<Image>().color = podium[i];

                    foreach (TMP_Text text in row.GetComponentsInChildren<TMP_Text>())
                        text.color = Color.black;
                }
            }
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Reading Leaderboard", ex, "Could not read the XML leaderboard of " + kahootNames[index]);
            statusText.text = "La clasificación está dañada o no está disponible.";
        }
    }

    public void BackToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}
