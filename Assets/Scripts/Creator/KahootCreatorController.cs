using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootCreatorController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_InputField titleInput;
    [SerializeField] private Transform contentParent;
    [SerializeField] private QuestionEditorItem questionPrefab;
    [SerializeField] private Button addQuestionButton;
    [SerializeField] private Button saveButton;
    [SerializeField] private TMP_Text statusText;

    private readonly List<QuestionEditorItem> questions = new List<QuestionEditorItem>();

    private void Start()
    {
        addQuestionButton.onClick.AddListener(AddQuestion);
        saveButton.onClick.AddListener(SaveKahoot);

        AddQuestion();
    }

    public void AddQuestion()
    {
        QuestionEditorItem item = Instantiate(questionPrefab, contentParent);
        item.Initialize(questions.Count + 1, RemoveQuestion);
        questions.Add(item);
    }

    private void RemoveQuestion(QuestionEditorItem item)
    {
        if (questions.Count <= 1)
        {
            statusText.text = "Un Kahoot debe tener al menos una pregunta.";
            return;
        }

        questions.Remove(item);
        Destroy(item.gameObject);

        for (int i = 0; i < questions.Count; i++)
            questions[i].SetNumber(i + 1);
    }

    public void SaveKahoot()
    {
        try
        {
            KahootData data = new KahootData
            {
                title = titleInput.text.Trim(),
                questions = new List<KahootQuestion>()
            };

            KahootManager.ToFileId(data.title);   // titulo obligatorio y valido como nombre de archivo

            foreach (QuestionEditorItem item in questions)
                data.questions.Add(item.GetQuestion());

            string id = KahootManager.SaveKahoot(data);
            statusText.text = "Kahoot '" + id + "' guardado. Ya puedes jugarlo desde el selector.";
        }
        catch (FormatException ex)
        {
            // Error del usuario (campo vacio, titulo repetido...): solo se muestra en pantalla.
            statusText.text = ex.Message;
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Kahoot Creator", ex, "Could not create the questionnaire.");
            statusText.text = "No se pudo guardar el Kahoot.";
        }
    }

    public void BackToMenu()
    {
        SceneLoader.Load("MainMenu");
    }

    public void OpenSelector()
    {
        SceneLoader.Load("KahootSelector");
    }
}
