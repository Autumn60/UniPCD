using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

namespace UniPCD.Editor
{
  public class PCDMultipleImporterEditorWindow : EditorWindow
  {
    [MenuItem("UniPCD/Import .pcd directory")]
    public static void ShowWindow()
    {
      GetWindow<PCDMultipleImporterEditorWindow>("PCD Multiple Importer");
    }

    private void OnGUI()
    {
      GUILayout.Label("Import .pcd directory", EditorStyles.boldLabel);

      if (GUILayout.Button("Import"))
      {
        string path = EditorUtility.OpenFolderPanel("Import .pcd directory", "", "");
        if (path.Length != 0)
        {
          string[] files = Directory.GetFiles(path, "*.pcd");
          List<string> importedFiles = new List<string>();
          foreach (string file in files)
          {
            if (PCDImporter.Import(file))
            {
              string fileName = Path.GetFileNameWithoutExtension(file);
              importedFiles.Add(fileName);
            }
            else
            {
              Debug.LogError($"Failed to read {file}");
            }
          }
          if (importedFiles.Count > 0)
          {
            Debug.Log($"Imported {string.Join(", ", importedFiles.ToArray())}");
          }
        }
      }
    }
  }
}
