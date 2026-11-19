using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionEditorItem : MonoBehaviour
{
    [SerializeField] private TMP_Text questionTitle;
    [SerializeField] private TMP_InputField questionInput;
    [SerializeField] private TMP_InputField[] answerInputs;
    [SerializeField] private TMP_Dropdown correctAnswerDropdown;
    [SerializeField] private TMP_InputField durationInput;
    [SerializeField] private Button removeButton;

    private Action<QuestionEditorItem> onRemove;

    public void Initialize(int number, Action<QuestionEditorItem> removeCallback)
    {
        onRemove = removeCallback;

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(() => onRemove?.Invoke(this));

        durationInput.text = "15";
        SetNumber(number);
    }

    public void SetNumber(int number)
    {
        questionTitle.text = "PREGUNTA " + number;
    }

    public KahootQuestion GetQuestion()
    {
        KahootQuestion question = new KahootQuestion();

        question.questionText = questionInput.text.Trim();
        question.answers = new string[answerInputs.Length];

        for (int i = 0; i < answerInputs.Length; i++)
            question.answers[i] = answerInputs[i].text.Trim();

        question.correctAnswerIndex = correctAnswerDropdown.value;

        if (!float.TryParse(durationInput.text.Replace(',', '.'), NumberStyles.Float, CultureInfo.InvariantCulture, out float duration))
            throw new FormatException("La duración de la pregunta no es válida.");

        question.durationSeconds = duration;

        return question;
    }
}
