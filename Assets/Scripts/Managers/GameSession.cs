/// <summary>Temporary data shared between scenes.</summary>
public static class GameSession
{
    // Selected questionnaire. SelectedKahootId is the JSON file name (without extension)
    // and is also the name of the XML leaderboard, so both always match.
    public static KahootData SelectedKahoot;
    public static string SelectedKahootId;
    public static string CurrentUsername;

    // Results of the last match.
    public static int FinalScore;
    public static int CorrectAnswers;
    public static int IncorrectAnswers;
    public static float TotalTime;
    public static int QuestionCount;
    public static int CurrentQuestionIndex;

    // Highscores scene: which leaderboard should be opened first (null = first available).
    public static string LeaderboardKahootId;

    // Report viewer.
    public static string SelectedReportPath;

    public static void ResetMatch()
    {
        FinalScore = 0;
        CorrectAnswers = 0;
        IncorrectAnswers = 0;
        TotalTime = 0f;
        CurrentQuestionIndex = 0;
        QuestionCount = SelectedKahoot != null && SelectedKahoot.questions != null
            ? SelectedKahoot.questions.Count
            : 0;
    }
}
