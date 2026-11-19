using System;
using System.Collections.Generic;

/// <summary>Data model of a questionnaire (serialised as JSON).</summary>
[Serializable]
public class KahootData
{
    public string title;
    public string description;
    public List<KahootQuestion> questions;
}

/// <summary>Data model of a single question.</summary>
[Serializable]
public class KahootQuestion
{
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex;
    public float durationSeconds;
}
