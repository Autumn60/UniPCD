using UnityEngine;
using UnityEditor;
using UniPCD.IO;
using System.IO;

namespace UniPCD.Editor
{
  public static class PCDImporter
  {
    public static bool Import(string path)
    {
      if (!PCDReader.Read(path, out PCD pcd)) return false;
      string fileName = Path.GetFileNameWithoutExtension(path);
      PCDObject pcdObject = ScriptableObject.CreateInstance<PCDObject>();
      pcdObject.pcd = pcd;
      AssetDatabase.CreateAsset(pcdObject, $"Assets/{fileName}.asset");
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();
      return true;
    }
  }
}