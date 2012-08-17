﻿using System;

namespace Cyotek.Data.Nbt
{
  public static class TagFactory
  {
    #region  Public Class Methods

    public static ITag CreateTag(TagType tagType)
    {
      return TagFactory.CreateTag(tagType, null);
    }

    public static ITag CreateTag(TagType tagType, object defaultValue)
    {
      return TagFactory.CreateTag(tagType, null, defaultValue);
    }

    public static ITag CreateTag(TagType tagType, string name, object defaultValue)
    {
      ITag result;

      switch (tagType)
      {
        case TagType.Byte:
          result = new TagByte();
          break;
        case TagType.Short:
          result = new TagShort();
          break;
        case TagType.Int:
          result = new TagInt();
          break;
        case TagType.Long:
          result = new TagLong();
          break;
        case TagType.Float:
          result = new TagFloat();
          break;
        case TagType.Double:
          result = new TagDouble();
          break;
        case TagType.ByteArray:
          result = new TagByteArray();
          break;
        case TagType.String:
          result = new TagString();
          break;
        case TagType.List:
          result = new TagList();
          break;
        case TagType.Compound:
          result = new TagCompound();
          break;
        case TagType.IntArray:
          result = new TagIntArray();
          break;
        default:
          throw new ArgumentException("Invalid tag type.", "tagType");
      }

      result.Name = name;
      if (defaultValue != null)
        result.Value = defaultValue;

      return result;
    }

    #endregion  Public Class Methods
  }
}
