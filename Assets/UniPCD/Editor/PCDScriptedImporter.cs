using System.IO;
using UnityEngine;
using UnityEditor.AssetImporters;
using UniPCD.IO;

namespace UniPCD.Editor
{
  [ScriptedImporter(1, "pcd")]
  public class PCDScriptedImporter : ScriptedImporter
  {
    public override void OnImportAsset(AssetImportContext ctx)
    {
      if (Path.GetExtension(ctx.assetPath) == ".pcd")
      {
        string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", ctx.assetPath));
        if (PCDReader.Read(fullPath, out PCD pcd))
        {
          PCDObject pcdObject = ScriptableObject.CreateInstance<PCDObject>();
          pcdObject.pcd = pcd;
          ctx.AddObjectToAsset("pcd", pcdObject);
          ctx.SetMainObject(pcdObject);
        }
        else
        {
          Debug.LogError("Failed to read .pcd file");
        }
      }
    }
  }
}