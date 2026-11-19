using System;
using System.Collections.Generic;

[Serializable]
public class KahootData
{
    public string title;
    public string description;
    public List<KahootQuestion> questions;
}

[Serializable]
public class KahootQuestion
{
    public string questionText;
    public string[] answers;
    public int correctAnswerIndex;
    public float durationSeconds;
}
