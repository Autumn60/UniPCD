using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace UniPCD.IO
{
  using FieldInfo = PCD.Header.FieldInfo;
  public static partial class PCDReader
  {
    private static string ReadLine(Stream stream)
    {
      StringBuilder builder = new StringBuilder();
      bool skip = false;
      while (true)
      {
        int c = stream.ReadByte();
        if (c == '\n' || c < 0) break;
        if (skip) continue;
        if (c == '#')
        {
          skip = true;
          continue;
        }
        builder.Append((char)c);
      }
      return builder.ToString().Trim();
    }

    private static bool ReadHeader(Stream stream, out PCD.Header header)
    {
      header = new PCD.Header();
      List<FieldInfo> fieldList = new List<FieldInfo>();

      bool breakFlag = false;
      while (stream.Position < stream.Length)
      {
        string line = ReadLine(stream);
        if (line == null || line == "") continue;
        string[] fields = line.Split(' ');
        if (fields.Length <= 0 || fields[0].Length <= 0 || fields[0][0] == '#') continue;
        switch (fields[0])
        {
          case "VERSION":
            header.version = fields[1];
            break;
          case "FIELDS":
            for (int i = 1; i < fields.Length; i++)
            {
              FieldInfo field = new FieldInfo();
              field.count = 1;

              string fieldName = fields[i].ToLower();
              field.name = fieldName;
              switch (fieldName)
              {
                case "x": field.field = FieldInfo.Field.X; break;
                case "y": field.field = FieldInfo.Field.Y; break;
                case "z": field.field = FieldInfo.Field.Z; break;
                case "rgb": field.field = FieldInfo.Field.RGB; break;
                default: field.field = FieldInfo.Field.Unknown; break;
              }
              fieldList.Add(field);
            }
            break;
          case "SIZE":
            int sizeCount = fields.Length - 1;
            if (sizeCount != fieldList.Count)
            {
              Debug.LogError("Field count mismatch");
              sizeCount = Mathf.Min(sizeCount, fieldList.Count);
            }
            for (int i = 0; i < sizeCount; i++)
            {
              fieldList[i].size = int.Parse(fields[i + 1]);
            }
            break;
          case "TYPE":
            int typeCount = fields.Length - 1;
            if (typeCount != fieldList.Count)
            {
              Debug.LogError("Field count mismatch");
              typeCount = Mathf.Min(typeCount, fieldList.Count);
            }
            for (int i = 0; i < typeCount; i++)
            {
              string type = fields[i + 1];
              switch (type)
              {
                case "I": fieldList[i].type = FieldInfo.FieldType.I; break;
                case "U": fieldList[i].type = FieldInfo.FieldType.U; break;
                case "F": fieldList[i].type = FieldInfo.FieldType.F; break;
                default: fieldList[i].type = FieldInfo.FieldType.Unknown; break;
              }
            }
            break;
          case "COUNT":
            int countCount = fields.Length - 1;
            if (countCount != fieldList.Count)
            {
              Debug.LogError("Field count mismatch");
              countCount = Mathf.Min(countCount, fieldList.Count);
            }
            for (int i = 0; i < countCount; i++)
            {
              fieldList[i].count = int.Parse(fields[i + 1]);
            }
            break;
          case "WIDTH":
            header.width = long.Parse(fields[1]);
            break;
          case "HEIGHT":
            header.height = long.Parse(fields[1]);
            break;
          case "POINTS":
            header.points = long.Parse(fields[1]);
            break;
          case "DATA":
            string format = fields[1];
            switch (format)
            {
              case "ascii": header.format = PCD.Header.DataFormat.ASCII; break;
              case "binary": header.format = PCD.Header.DataFormat.Binary; break;
              default: header.format = PCD.Header.DataFormat.Unknown; break;
            }
            breakFlag = true;
            break;
        }
        if (breakFlag) break;
      }
      header.fields = fieldList.ToArray();
      return true;
    }
  }
}
