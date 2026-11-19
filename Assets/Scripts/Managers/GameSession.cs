public static class GameSession
{
    public static KahootData SelectedKahoot;
    public static string SelectedKahootId;
    public static string CurrentUsername;

    public static int FinalScore;
    public static int CorrectAnswers;
    public static int IncorrectAnswers;
    public static float TotalTime;
    public static int QuestionCount;
    public static int CurrentQuestionIndex;

    public static string LeaderboardKahootId;

    public static string SelectedReportPath;

    public static void ResetMatch()
    {
        FinalScore = 0;
        CorrectAnswers = 0;
        IncorrectAnswers = 0;
        TotalTime = 0f;
        CurrentQuestionIndex = 0;
        QuestionCount = SelectedKahoot != null && SelectedKahoot.questions != null ? SelectedKahoot.questions.Count : 0;
    }
}
