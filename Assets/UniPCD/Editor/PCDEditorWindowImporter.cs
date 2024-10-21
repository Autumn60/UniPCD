using UnityEngine;
using UnityEditor;
using UniPCD.IO;
using System.IO;

namespace UniPCD.Editor
{
  public class PCDEditorWindowImporter : EditorWindow
  {
    [MenuItem("UniPCD/Import .pcd file")]
    public static void ShowWindow()
    {
      GetWindow<PCDEditorWindowImporter>("PCD Importer");
    }

    private void OnGUI()
    {
      GUILayout.Label("Import .pcd file", EditorStyles.boldLabel);

      if (GUILayout.Button("Import"))
      {
        string path = EditorUtility.OpenFilePanel("Import .pcd file", "", "pcd");
        if (path.Length != 0)
        {
          if (PCDReader.Read(path, out PCD pcd))
          {
            string fileName = Path.GetFileNameWithoutExtension(path);
            PCDObject pcdObject = ScriptableObject.CreateInstance<PCDObject>();
            pcdObject.pcd = pcd;
            AssetDatabase.CreateAsset(pcdObject, $"Assets/{fileName}.asset");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
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
