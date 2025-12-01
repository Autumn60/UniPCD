using UnityEngine;
using UnityEditor;
using UniPCD.IO;
using System.IO;
using System.Collections.Generic;

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

    public static bool Import(string[] paths, string fileName, int skipRate = 1)
    {
      PCD[] pcds = new PCD[paths.Length];
      long points = 0;
      for (int i = 0; i < paths.Length; i++)
      {
        if (!PCDReader.Read(paths[i], out pcds[i])) return false;
        points += (pcds[i].header.points + skipRate - 1) / skipRate;
      }

      PCD mergedPCD = new PCD();
      mergedPCD.header = pcds[0].header;
      mergedPCD.header.points = points;
      mergedPCD.header.width = points;
      mergedPCD.header.height = 1;
      mergedPCD.pointCloud = new PointCloud();
      List<Point> allPoints = new List<Point>((int)points);
      foreach (var pcd in pcds)
      {
        Point[] src = pcd.pointCloud.points;
        for (int i = 0; i < src.Length; i += skipRate)
        {
          allPoints.Add(src[i]);
        }
      }
      mergedPCD.pointCloud.points = allPoints.ToArray();
      PCDObject pcdObject = ScriptableObject.CreateInstance<PCDObject>();
      pcdObject.pcd = mergedPCD;
      AssetDatabase.CreateAsset(pcdObject, $"Assets/{fileName}.asset");
      AssetDatabase.SaveAssets();
      AssetDatabase.Refresh();

      return true;
    }
  }
}
