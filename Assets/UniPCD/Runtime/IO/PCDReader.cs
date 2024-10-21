using System.IO;

namespace UniPCD.IO
{
  using DataFormat = PCD.Header.DataFormat;

  public static partial class PCDReader
  {
    public static bool Read(string path, out PCD pcd)
    {
      using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
      {
        return Read(fs, out pcd);
      }
    }

    public static bool Read(Stream stream, out PCD pcd)
    {
      pcd = new PCD();
      if (!ReadHeader(stream, out pcd.header)) return false;
      switch (pcd.header.format)
      {
        case DataFormat.ASCII:
          break;
        case DataFormat.Binary:
          if (!ReadBinaryData(stream, pcd.header, out pcd.pointCloud)) return false;
          break;
        default:
          return false;
      }
      return true;
    }
  }
}
