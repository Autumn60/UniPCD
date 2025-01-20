using UnityEngine;
using UnityEditor;
using System.IO;

namespace UniPCD.Editor
{
  public class PCDImporterEditorWindow : EditorWindow
  {
    [MenuItem("UniPCD/Import .pcd file")]
    public static void ShowWindow()
    {
      GetWindow<PCDImporterEditorWindow>("PCD Importer");
    }

    private void OnGUI()
    {
      GUILayout.Label("Import .pcd file", EditorStyles.boldLabel);

      if (GUILayout.Button("Import"))
      {
        string path = EditorUtility.OpenFilePanel("Import .pcd file", "", "pcd");
        if (path.Length != 0)
        {
          if (PCDImporter.Import(path))
          {
            string fileName = Path.GetFileNameWithoutExtension(path);
            Debug.Log($"Imported {fileName}.asset");
          }
          else
          {
            Debug.LogError("Failed to read .pcd file");
          }
        }
      }
    }
  }
}
