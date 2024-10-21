using System;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

namespace UniPCD.IO
{
  public static partial class PCDReader
  {
    private static int CalculatePointSize(PCD.Header header)
    {
      int size = 0;
      foreach (var field in header.fields)
      {
        size += field.size * field.count;
      }
      return size;
    }

    private static bool ReadBinaryData(Stream stream, PCD.Header header, out PointCloud pointCloud)
    {
      pointCloud = new PointCloud();

      BinaryDataParser parser = BinaryDataParser.GenerateBinaryDataParser(header);

      const uint bufferPointCount = 4096;
      uint pointSize = (uint)CalculatePointSize(header);
      uint bufferSize = bufferPointCount * pointSize;
      byte[] buffer = new byte[bufferSize];

      long pointIndex = 0;
      long pointCount = header.points;
      List<Point> pointList = new List<Point>();
      while (pointIndex < pointCount)
      {
        uint readPointCount = (uint)Math.Min(bufferPointCount, pointCount - pointIndex);
        int readLength = (int)(readPointCount * pointSize);
        if (stream.Read(buffer, 0, readLength) != readLength) return false;

        ulong offset = 0;
        for (uint i = 0; i < readPointCount; i++)
        {
          pointList.Add(parser.Parse(buffer, offset));
          offset += pointSize;
        }

        pointIndex += readPointCount;
      }
      pointCloud.points = pointList.ToArray();

      return true;
    }
  }
}
