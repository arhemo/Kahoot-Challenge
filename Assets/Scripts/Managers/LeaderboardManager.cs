using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

/// <summary>Reads and writes one XML leaderboard per Kahoot (named after the Kahoot identifier).</summary>
public static class LeaderboardManager
{
    private static string GetLeaderboardPath(string kahootId)
    {
        string safeName = kahootId ?? "Unknown";

        foreach (char c in Path.GetInvalidFileNameChars())
            safeName = safeName.Replace(c, '_');

        return Path.Combine(KahootManager.LeaderboardsPath, safeName + ".xml");
    }

    /// <summary>Returns an empty leaderboard when no XML exists; throws if the XML is damaged.</summary>
    public static LeaderboardData LoadLeaderboard(string kahootId)
    {
        KahootManager.InitializeFolders();

        string path = GetLeaderboardPath(kahootId);

        if (!File.Exists(path))
            return new LeaderboardData();

        XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));

        using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            LeaderboardData data = (LeaderboardData)serializer.Deserialize(stream);

            if (data == null || data.entries == null)
                throw new InvalidDataException("Invalid leaderboard XML structure.");

            Sort(data);
            return data;
        }
    }

    public static void SaveLeaderboard(string kahootId, LeaderboardData data)
    {
        KahootManager.InitializeFolders();

        string path = GetLeaderboardPath(kahootId);
        string tempPath = path + ".tmp";

        XmlSerializer serializer = new XmlSerializer(typeof(LeaderboardData));

        using (FileStream stream = new FileStream(tempPath, FileMode.Create, FileAccess.Write))
            serializer.Serialize(stream, data);

        // The previous XML is only discarded once the new one has been written completely.
        if (File.Exists(path))
            File.Delete(path);

        File.Move(tempPath, path);
    }

    /// <summary>Adds a result and returns its 1-based position in the ranking.</summary>
    public static int AddResult(string kahootId, ScoreEntry newEntry, out int totalEntries)
    {
        LeaderboardData leaderboard;

        try
        {
            leaderboard = LoadLeaderboard(kahootId);
        }
        catch (Exception ex)
        {
            // Damaged XML: report it, keep a backup of the broken file and start a new ranking.
            ExceptionManager.Log("Leaderboard", ex,
                "The XML of '" + kahootId + "' is damaged. It was backed up as .corrupt and a new ranking was created.");

            BackupCorruptFile(kahootId);
            leaderboard = new LeaderboardData();
        }

        leaderboard.entries.Add(newEntry);
        Sort(leaderboard);

        SaveLeaderboard(kahootId, leaderboard);

        totalEntries = leaderboard.entries.Count;
        return leaderboard.entries.IndexOf(newEntry) + 1;
    }

    /// <summary>Highest score first; ties are resolved by the shortest total time.</summary>
    public static void Sort(LeaderboardData data)
    {
        data.entries = data.entries
            .OrderByDescending(e => e.score)
            .ThenBy(e => e.totalTime)
            .ToList();
    }

    private static void BackupCorruptFile(string kahootId)
    {
        try
        {
            string path = GetLeaderboardPath(kahootId);

            if (!File.Exists(path))
                return;

            string backup = path + "." + DateTime.Now.ToString("yyyyMMdd_HHmmss") + ".corrupt";
            File.Move(path, backup);
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.LogWarning("Could not back up the corrupt leaderboard: " + ex.Message);
        }
    }
}
