using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace UniPCD.IO
{
  using Header = PCD.Header;
  using Field = PCD.Header.FieldInfo.Field;
  using FieldType = PCD.Header.FieldInfo.FieldType;

  public static partial class PCDReader
  {
    private class BinaryDataParser
    {
      private delegate void ParseBinaryData(byte[] data, ulong offset, ref Point point);
      private ParseBinaryData[] _parsingFunctions;
      private ulong[] _fieldOffsets;

      public static BinaryDataParser GenerateBinaryDataParser(Header header)
      {
        BinaryDataParser parser = new BinaryDataParser();
        List<ParseBinaryData> parsingFunctionList = new List<ParseBinaryData>();

        parser._fieldOffsets = new ulong[header.fields.Length];
        ulong fieldOffset = 0;
        for (int i = 0; i < header.fields.Length; i++)
        {
          parser._fieldOffsets[i] = fieldOffset;
          int fieldSize = header.fields[i].size;
          fieldOffset += (ulong)fieldSize;

          Field field = header.fields[i].field;
          FieldType fieldType = header.fields[i].type;
          if (field == Field.Unknown || fieldType == FieldType.Unknown)
          {
            parsingFunctionList.Add(ReadUnknown);
          }
          else
          {
            parsingFunctionList.Add(s_ParseBinaryDataFunctions[field][fieldType][fieldSize]);
          }
        }
        parser._parsingFunctions = parsingFunctionList.ToArray();
        return parser;
      }

      public Point Parse(byte[] data, ulong offset)
      {
        Point point = new Point();
        for (int i = 0; i < _parsingFunctions.Length; i++)
        {
          _parsingFunctions[i](data, offset + _fieldOffsets[i], ref point);
        }
        return point;
      }

      private static Dictionary<Field, Dictionary<FieldType, Dictionary<int, ParseBinaryData>>> s_ParseBinaryDataFunctions = new Dictionary<Field, Dictionary<FieldType, Dictionary<int, ParseBinaryData>>>
      {
        {
          Field.X, new Dictionary<FieldType, Dictionary<int, ParseBinaryData>>
          {
            { FieldType.F, new Dictionary<int, ParseBinaryData> { { 4, ReadX_F4 } } }
          }
        },
        {
          Field.Y, new Dictionary<FieldType, Dictionary<int, ParseBinaryData>>
          {
            { FieldType.F, new Dictionary<int, ParseBinaryData> { { 4, ReadY_F4 } } }
          }
        },
        {
          Field.Z, new Dictionary<FieldType, Dictionary<int, ParseBinaryData>>
          {
            { FieldType.F, new Dictionary<int, ParseBinaryData> { { 4, ReadZ_F4 } } }
          }
        },
        {
          Field.RGB, new Dictionary<FieldType, Dictionary<int, ParseBinaryData>>
          {
            { FieldType.F, new Dictionary<int, ParseBinaryData> { { 4, ReadRGB_F4 } } }
          }
        }
      };

      [StructLayout(LayoutKind.Explicit)]
      private struct UintFloatUnion
      {
        [FieldOffset(0)] public uint u;
        [FieldOffset(0)] public float f;
      }

      private static void ReadUnknown(byte[] binary, ulong offset, ref Point point) { }

      private static float UintToFloat(uint value)
      {
        UintFloatUnion union = new UintFloatUnion();
        union.u = value;
        return union.f;
      }
      private static void ReadX_F4(byte[] binary, ulong offset, ref Point point) { point.position.x = UintToFloat((uint)binary[offset + 0] + ((uint)binary[offset + 1] << 8) + ((uint)binary[offset + 2] << 16) + ((uint)binary[offset + 3] << 24)); }
      private static void ReadY_F4(byte[] binary, ulong offset, ref Point point) { point.position.y = UintToFloat((uint)binary[offset + 0] + ((uint)binary[offset + 1] << 8) + ((uint)binary[offset + 2] << 16) + ((uint)binary[offset + 3] << 24)); }
      private static void ReadZ_F4(byte[] binary, ulong offset, ref Point point) { point.position.z = UintToFloat((uint)binary[offset + 0] + ((uint)binary[offset + 1] << 8) + ((uint)binary[offset + 2] << 16) + ((uint)binary[offset + 3] << 24)); }
      private static void ReadRGB_F4(byte[] binary, ulong offset, ref Point point) { point.color.r = binary[offset]; point.color.g = binary[offset + 1]; point.color.b = binary[offset + 2]; point.color.a = binary[offset + 3]; }
    }
  }
}