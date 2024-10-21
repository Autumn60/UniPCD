using System;

namespace UniPCD
{
  public partial class PCD
  {
    [Serializable]
    public partial class Header
    {
      public string version;
      public FieldInfo[] fields;
      public long width;
      public long height;
      public long points;
      public DataFormat format;
    }
  }
}