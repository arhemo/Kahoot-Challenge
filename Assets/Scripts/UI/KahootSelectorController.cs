using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootSelectorController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField usernameInput;
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject kahootCardPrefab;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        usernameInput.text = PlayerPrefs.GetString("Username", "Jugador");
        RefreshKahoots();
    }

    // Se lee la carpeta cada vez, asi los Kahoots nuevos aparecen sin reiniciar
    public void RefreshKahoots()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        statusText.text = "";

        try
        {
            var kahoots = KahootManager.GetAvailableKahoots();

            foreach (string kahootName in kahoots)
            {
                GameObject card = Instantiate(kahootCardPrefab, contentParent);

                TMP_Text title = card.transform.Find("Texts/KahootTitle").GetComponent<TMP_Text>();
                TMP_Text info = card.transform.Find("Texts/QuestionCount").GetComponent<TMP_Text>();
                Button select = card.transform.Find("SelectButton").GetComponent<Button>();

                title.text = kahootName;

                try
                {
                    KahootData data = KahootManager.LoadKahoot(kahootName);
                    info.text = data.questions.Count + " preguntas";

                    if (!string.IsNullOrWhiteSpace(data.description))
                        info.text += "  |  " + data.description;

                    select.onClick.AddListener(() => SelectKahoot(kahootName));
                }
                catch (Exception ex)
                {
                    // Un Kahoot roto no impide ver los demás
                    ExceptionManager.Log("Kahoot Selector", ex, "Invalid Kahoot file: " + kahootName);
                    info.text = "Archivo no válido (mira Informes de error)";
                    select.GetComponentInChildren<TMP_Text>().text = "NO VÁLIDO";
                    select.interactable = false;
                }
            }

            if (kahoots.Count == 0)
                statusText.text = "No hay Kahoots disponibles.";
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Kahoot Selector", ex, "Could not load the Kahoot list.");
            statusText.text = "No se pudieron cargar los cuestionarios.";
        }
    }

    private void SelectKahoot(string kahootName)
    {
        if (string.IsNullOrWhiteSpace(usernameInput.text))
        {
            statusText.text = "Escribe tu nombre primero.";
            return;
        }

        try
        {
            GameSession.SelectedKahoot = KahootManager.LoadKahoot(kahootName);
            GameSession.SelectedKahootId = kahootName;
            GameSession.CurrentUsername = usernameInput.text.Trim();
            PlayerPrefs.SetString("Username", GameSession.CurrentUsername);
            GameSession.ResetMatch();

            SceneLoader.Load("KahootGame");
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Loading Kahoot", ex, "Could not load questionnaire: " + kahootName);
            statusText.text = "Este Kahoot no es válido o no se puede abrir.";
        }
    }

    public void BackToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}
