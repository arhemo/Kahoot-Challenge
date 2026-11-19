
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

[Serializable]
[XmlRoot("Leaderboard")]
public class LeaderboardData
{
    [XmlElement("Entry")]
    public List<ScoreEntry> entries = new List<ScoreEntry>();
}

[Serializable]
public class ScoreEntry
{
    public string username;
    public int score;
    public float totalTime;
    public int correctAnswers;
    public string date;
}