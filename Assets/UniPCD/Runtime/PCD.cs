using System;

using UnityEngine;

namespace UniPCD
{
  [Serializable]
  public partial class PCD
  {
    public Header header;
    [HideInInspector]
    public PointCloud pointCloud;
  }
}