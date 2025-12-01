using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace UniPCD.Editor
{
  public class PCDMultipleImporterEditorWindow : EditorWindow
  {
    private string _fileName = "MergedPCD";
    private int _skipRate = 1;

    [MenuItem("UniPCD/Import .pcd directory")]
    public static void ShowWindow()
    {
      GetWindow<PCDMultipleImporterEditorWindow>("PCD Multiple Importer");
    }

    private void OnGUI()
    {
      GUILayout.Label("Import .pcd directory", EditorStyles.boldLabel);

      _fileName = EditorGUILayout.TextField("File Name", _fileName);
      _skipRate = EditorGUILayout.IntSlider("Skip Rate", _skipRate, 1, 1000);

      if (GUILayout.Button("Import"))
      {
        string path = EditorUtility.OpenFolderPanel("Import .pcd directory", "", "");
        if (path.Length != 0)
        {
          string[] files = Directory.GetFiles(path, "*.pcd");
          if (PCDImporter.Import(files, _fileName, _skipRate))
          {
            Debug.Log($"Imported {_fileName}.asset (skip rate: {_skipRate})");
          }
          else
          {
            Debug.LogError("Failed to read .pcd files");
          }
        }
      }
    }
  }
}
