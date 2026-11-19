using System;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ExceptionManager
{
    public static void Log(string context, Exception exception, string developerMessage)
    {
        try
        {
            KahootManager.InitializeFolders();

            DateTime now = DateTime.Now;
            string fileName = "Exception_" + now.ToString("yyyyMMdd_HHmmss_fff") + ".txt";
            string path = Path.Combine(KahootManager.ReportsPath, fileName);

            StringBuilder report = new StringBuilder();
            report.AppendLine("KAHOOT APPLICATION - ERROR REPORT");
            report.AppendLine("=================================");
            report.AppendLine("Date: " + now.ToString("yyyy-MM-dd"));
            report.AppendLine("Time: " + now.ToString("HH:mm:ss.fff"));
            report.AppendLine("Context: " + context);
            report.AppendLine("Problem: " + developerMessage);
            report.AppendLine("Application version: " + Application.version + " (Unity " + Application.unityVersion + ")");
            report.AppendLine("Platform: " + Application.platform);
            report.AppendLine();

            if (exception != null)
            {
                report.AppendLine("Exception type: " + exception.GetType().Name);
                report.AppendLine("Exception message: " + exception.Message);

                if (exception.InnerException != null)
                    report.AppendLine("Inner exception: " + exception.InnerException.Message);

                report.AppendLine();
                report.AppendLine("Stack Trace:");
                report.AppendLine(exception.StackTrace);
            }

            File.WriteAllText(path, report.ToString());
            Debug.LogWarning("Exception report generated: " + path);
        }
        catch (Exception loggingException)
        {
            Debug.LogError("Could not create error report: " + loggingException.Message);
        }
    }

    public static string[] GetReports()
    {
        KahootManager.InitializeFolders();

        return Directory.GetFiles(KahootManager.ReportsPath, "*.txt").OrderByDescending(p => Path.GetFileName(p), StringComparer.Ordinal).ToArray();
    }

    public static string ReadReport(string path)
    {
        if (string.IsNullOrEmpty(path) || !File.Exists(path))
            throw new FileNotFoundException("The selected report does not exist.", path);

        return File.ReadAllText(path);
    }
}
