using System;
using UnityEngine;

public class AppInitializer : MonoBehaviour
{
    private void Awake()
    {
        KahootManager.InitializeFolders();
        Debug.Log("Application data path: " + Application.persistentDataPath);
    }
}