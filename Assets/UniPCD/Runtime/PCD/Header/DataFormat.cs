using System;

namespace UniPCD
{
  public partial class PCD
  {
    public partial class Header
    {
      [Serializable]
      public enum DataFormat
      {
        Unknown,
        ASCII,
        Binary
      }
    }
  }
}