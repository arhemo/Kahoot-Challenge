using System;
using UnityEngine;
using TMPro;

public class ReportViewerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text reportText;

    private void Start()
    {
        try
        {
            reportText.text = ExceptionManager.ReadReport(GameSession.SelectedReportPath);
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Report Viewer", ex, "Could not read the selected error report.");
            reportText.text = "No se pudo abrir el informe seleccionado.";
        }
    }

    public void BackToReports()
    {
        SceneLoader.Load("ReportsList");
    }

    public void BackToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}
