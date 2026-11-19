using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReportsListUI : MonoBehaviour
{
    [SerializeField] private Transform contentParent;
    [SerializeField] private Button reportButtonPrefab;
    [SerializeField] private TMP_Text statusText;

    private void Start()
    {
        LoadReports();
    }

    public void LoadReports()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        try
        {
            string[] reports = ExceptionManager.GetReports();

            if (reports.Length == 0)
            {
                statusText.text = "No hay informes de error.";
                return;
            }

            statusText.text = "";

            foreach (string reportPath in reports)
            {
                Button button = Instantiate(reportButtonPrefab, contentParent);
                button.GetComponentInChildren<TMP_Text>().text = Path.GetFileName(reportPath);

                string path = reportPath;
                button.onClick.AddListener(() =>
                {
                    GameSession.SelectedReportPath = path;
                    SceneLoader.Load("ReportViewer");
                });
            }
        }
        catch (Exception ex)
        {
            ExceptionManager.Log("Reports List", ex, "Could not load the error reports.");
            statusText.text = "No se pudieron cargar los informes.";
        }
    }

    public void BackToMenu()
    {
        SceneLoader.Load("MainMenu");
    }
}
