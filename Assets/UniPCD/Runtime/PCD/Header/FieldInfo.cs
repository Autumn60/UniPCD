using System;

namespace UniPCD
{
  public partial class PCD
  {
    public partial class Header
    {
      [Serializable]
      public partial class FieldInfo
      {
        public string name;
        public Field field;
        public FieldType type;
        public int size;
        public int count;
      }
    }
  }
}