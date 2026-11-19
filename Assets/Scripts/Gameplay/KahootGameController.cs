using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KahootGameController : MonoBehaviour
{
    [Header("Question UI")]
    [SerializeField] private TMP_Text questionText;
    [SerializeField] private TMP_Text questionCounter;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text feedbackText;

    [Header("Answers")]
    [SerializeField] private Button[] answerButtons;
    [SerializeField] private Slider timerSlider;
    [SerializeField] private Color[] answerColors;

    [Header("Colors")]
    [SerializeField] private Color correctColor = new Color(0.149f, 0.537f, 0.047f);
    [SerializeField] private Color dimColor = new Color(1f, 1f, 1f, 0.25f);

    [Header("Sound")]
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip[] correctClips;
    [SerializeField] private AudioClip[] incorrectClips;
    [SerializeField] private AudioClip newQuestionClip;
    [SerializeField] private AudioClip tickClip;
    [SerializeField] private AudioClip pointsClip;
    [SerializeField] private AudioClip endClip;

    private KahootData kahoot;
    private int currentIndex;
    private float remainingTime;
    private float questionDuration;
    private bool answering = true;
    private int score;
    private int lastSecond;

    private void Start()
    {
        kahoot = GameSession.SelectedKahoot;

        if (kahoot == null || kahoot.questions == null || kahoot.questions.Count == 0)
        {
            SceneLoader.Load("KahootSelector");
            return;
        }

        GameSession.ResetMatch();
        ShowQuestion();
    }

    private void Update()
    {
        if (answering)
            return;

        remainingTime -= Time.deltaTime;

        if (remainingTime <= 0)
        {
            remainingTime = 0;
            UpdateTimerUI();
            AnswerQuestion(-1);
            return;
        }

        UpdateTimerUI();
    }

    private void ShowQuestion()
    {
        if (currentIndex >= kahoot.questions.Count)
        {
            GameSession.FinalScore = score;
            PlayClip(endClip);
            SceneLoader.Load("Results");
            return;
        }

        KahootQuestion question = kahoot.questions[currentIndex];

        questionDuration = question.durationSeconds;
        remainingTime = questionDuration;
        answering = false;

        questionText.text = question.questionText;
        questionCounter.text = "PREGUNTA " + (currentIndex + 1) + " / " + kahoot.questions.Count;
        scoreText.text = "PUNTOS: " + score;
        feedbackText.text = "";

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int answerIndex = i;

            answerButtons[i].interactable = true;
            answerButtons[i].GetComponent<Image>().color = answerColors[i];
            answerButtons[i].GetComponentInChildren<TMP_Text>().text = question.answers[i];

            // Evento nuevo: borra tambien lo que hubiera conectado en el Inspector.
            answerButtons[i].onClick = new Button.ButtonClickedEvent();
            answerButtons[i].onClick.AddListener(() => AnswerQuestion(answerIndex));
        }

        UpdateTimerUI();
        PlayClip(newQuestionClip);
    }

    private void UpdateTimerUI()
    {
        float ratio = questionDuration > 0 ? remainingTime / questionDuration : 0;
        int second = Mathf.CeilToInt(remainingTime);

        timerText.text = second.ToString();
        timerSlider.value = ratio;

        // Verde -> amarillo -> rojo segun el tiempo que queda
        Image fill = timerSlider.fillRect.GetComponent<Image>();
        fill.color = ratio > 0.5f ? answerColors[3] : (ratio > 0.25f ? answerColors[2] : answerColors[0]);
        timerText.color = remainingTime <= 5f ? Color.red : Color.white;

        if (!answering && remainingTime <= 5f && second != lastSecond && second > 0)
            PlayClip(tickClip);

        lastSecond = second;
    }

    private void AnswerQuestion(int selectedIndex)
    {
        if (answering)
            return;

        answering = true;

        int correctIndex = kahoot.questions[currentIndex].correctAnswerIndex;
        GameSession.TotalTime += questionDuration - remainingTime;

        if (selectedIndex == correctIndex)
        {
            GameSession.CorrectAnswers++;

            // Más puntos cuanto más tiempo queda
            int points = Mathf.CeilToInt(1000 * (remainingTime / questionDuration));
            score += points;

            feedbackText.text = "¡CORRECTO! +" + points + " PUNTOS";
            feedbackText.color = new Color(0.5f, 1f, 0.5f);
            PlayClip(correctClips[Random.Range(0, correctClips.Length)]);
            PlayClip(pointsClip);
        }
        else
        {
            GameSession.IncorrectAnswers++;

            feedbackText.text = selectedIndex == -1 ? "¡SE ACABÓ EL TIEMPO!" : "¡INCORRECTO!";
            feedbackText.color = new Color(1f, 0.55f, 0.55f);
            PlayClip(incorrectClips[Random.Range(0, incorrectClips.Length)]);
        }

        // Como en Kahoot: las incorrectas se apagan y la correcta se resalta
        for (int i = 0; i < answerButtons.Length; i++)
        {
            answerButtons[i].interactable = false;
            answerButtons[i].GetComponent<Image>().color = i == correctIndex ? correctColor : dimColor;
        }

        GameSession.FinalScore = score;
        scoreText.text = "PUNTOS: " + score;

        StartCoroutine(NextQuestion());
    }

    private IEnumerator NextQuestion()
    {
        yield return new WaitForSeconds(1.5f);
        currentIndex++;
        ShowQuestion();
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
            sfxSource.PlayOneShot(clip);
    }
}
