using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>Loads, validates and saves questionnaires (JSON) and owns the data folders.</summary>
public static class KahootManager
{
    public const int MaxTitleLength = 60;

    public static string KahootsPath => Path.Combine(Application.persistentDataPath, "Kahoots");
    public static string LeaderboardsPath => Path.Combine(Application.persistentDataPath, "Leaderboards");
    public static string ReportsPath => Path.Combine(Application.persistentDataPath, "ErrorReports");

    private static readonly string[] ReservedNames =
    {
        "CON", "PRN", "AUX", "NUL", "COM1", "COM2", "COM3", "COM4", "COM5", "COM6", "COM7", "COM8", "COM9",
        "LPT1", "LPT2", "LPT3", "LPT4", "LPT5", "LPT6", "LPT7", "LPT8", "LPT9"
    };

    public static void InitializeFolders()
    {
        Directory.CreateDirectory(KahootsPath);
        Directory.CreateDirectory(LeaderboardsPath);
        Directory.CreateDirectory(ReportsPath);
    }

    /// <summary>Identifiers (file names without extension) of default and external Kahoots.</summary>
    public static List<string> GetAvailableKahoots()
    {
        InitializeFolders();

        List<string> result = new List<string>();

        TextAsset[] defaults = Resources.LoadAll<TextAsset>("DefaultKahoots");
        List<string> defaultNames = new List<string>();
        foreach (TextAsset asset in defaults)
            defaultNames.Add(asset.name);
        defaultNames.Sort(StringComparer.OrdinalIgnoreCase);

        foreach (string name in defaultNames)
            AddUnique(result, name);

        string[] externalFiles = Directory.GetFiles(KahootsPath, "*.json");
        List<string> externalNames = new List<string>();
        foreach (string file in externalFiles)
            externalNames.Add(Path.GetFileNameWithoutExtension(file));
        externalNames.Sort(StringComparer.OrdinalIgnoreCase);

        foreach (string name in externalNames)
            AddUnique(result, name);

        return result;
    }

    private static void AddUnique(List<string> list, string value)
    {
        foreach (string existing in list)
            if (string.Equals(existing, value, StringComparison.OrdinalIgnoreCase))
                return;

        list.Add(value);
    }

    /// <summary>True if a Kahoot (default or external) already uses this identifier.</summary>
    public static bool KahootExists(string id)
    {
        foreach (string existing in GetAvailableKahoots())
            if (string.Equals(existing, id, StringComparison.OrdinalIgnoreCase))
                return true;

        return false;
    }

    public static KahootData LoadKahoot(string kahootId)
    {
        InitializeFolders();

        string externalPath = Path.Combine(KahootsPath, kahootId + ".json");
        string json;
        string source;

        if (File.Exists(externalPath))
        {
            json = File.ReadAllText(externalPath);
            source = externalPath;
        }
        else
        {
            TextAsset asset = Resources.Load<TextAsset>("DefaultKahoots/" + kahootId);

            if (asset == null)
                throw new FileNotFoundException("Kahoot file was not found: " + kahootId);

            json = asset.text;
            source = "DefaultKahoots/" + kahootId;
        }

        KahootData data;

        try
        {
            data = JsonUtility.FromJson<KahootData>(json);
        }
        catch (ArgumentException ex)
        {
            throw new FormatException("Malformed JSON in '" + source + "': " + ex.Message, ex);
        }

        try
        {
            ValidateKahoot(data);
        }
        catch (FormatException ex)
        {
            throw new FormatException("Invalid Kahoot '" + source + "': " + ex.Message, ex);
        }

        return data;
    }

    /// <summary>Validates and saves a new Kahoot. Returns its identifier (file name).</summary>
    public static string SaveKahoot(KahootData data)
    {
        InitializeFolders();
        ValidateKahoot(data);

        string id = ToFileId(data.title);

        if (KahootExists(id))
            throw new FormatException("Ya existe un Kahoot llamado '" + id + "'. Elige otro título.");

        string path = Path.Combine(KahootsPath, id + ".json");
        string tempPath = path + ".tmp";
        string json = JsonUtility.ToJson(data, true);

        // Atomic write: a crash can never leave a half-written JSON behind.
        File.WriteAllText(tempPath, json);
        File.Move(tempPath, path);

        return id;
    }

    /// <summary>Checks that the title can be used as a file name and returns it trimmed.</summary>
    public static string ToFileId(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new FormatException("El título del Kahoot es obligatorio.");

        string id = title.Trim();

        if (id.Length > MaxTitleLength)
            throw new FormatException("El título no puede tener más de " + MaxTitleLength + " caracteres.");

        if (id.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            throw new FormatException("El título tiene caracteres no permitidos en un nombre de archivo ( \\ / : * ? \" < > | ).");

        if (id.EndsWith(".") || id.StartsWith("."))
            throw new FormatException("El título no puede empezar ni acabar en punto.");

        foreach (string reserved in ReservedNames)
            if (string.Equals(id, reserved, StringComparison.OrdinalIgnoreCase))
                throw new FormatException("'" + id + "' es un nombre reservado del sistema.");

        return id;
    }

    public static void ValidateKahoot(KahootData data)
    {
        if (data == null)
            throw new FormatException("La estructura del JSON no es válida.");

        if (string.IsNullOrWhiteSpace(data.title))
            throw new FormatException("Falta el título del Kahoot.");

        if (data.questions == null || data.questions.Count == 0)
            throw new FormatException("El Kahoot no tiene preguntas.");

        for (int i = 0; i < data.questions.Count; i++)
        {
            KahootQuestion question = data.questions[i];
            string prefix = "Pregunta " + (i + 1) + ": ";

            if (question == null || string.IsNullOrWhiteSpace(question.questionText))
                throw new FormatException(prefix + "falta el enunciado.");

            if (question.answers == null || question.answers.Length != 4)
                throw new FormatException(prefix + "debe tener exactamente cuatro respuestas.");

            foreach (string answer in question.answers)
                if (string.IsNullOrWhiteSpace(answer))
                    throw new FormatException(prefix + "hay una respuesta vacía.");

            if (question.correctAnswerIndex < 0 || question.correctAnswerIndex >= question.answers.Length)
                throw new FormatException(prefix + "la respuesta correcta no es válida.");

            if (question.durationSeconds <= 0 || float.IsNaN(question.durationSeconds) ||
                float.IsInfinity(question.durationSeconds))
                throw new FormatException(prefix + "la duración debe ser un número positivo.");
        }
    }
}
