using System;

namespace Cyotek.Data.Nbt
{
  public static class TagFactory
  {
    #region Static Methods

    public static ITag CreateTag(TagType tagType)
    {
      return CreateTag(tagType, null);
    }

    public static ITag CreateTag(TagType tagType, object value)
    {
      return CreateTag(tagType, string.Empty, value);
    }

    public static TagByte CreateTag(string name, byte value)
    {
      return new TagByte(name, value);
    }

    public static TagShort CreateTag(string name, short value)
    {
      return new TagShort(name, value);
    }

    public static TagIntArray CreateTag(string name, int[] value)
    {
      return new TagIntArray(name, value);
    }

    public static TagByteArray CreateTag(string name, byte[] value)
    {
      return new TagByteArray(name, value);
    }

    public static TagInt CreateTag(string name, int value)
    {
      return new TagInt(name, value);
    }

    public static TagLong CreateTag(string name, long value)
    {
      return new TagLong(name, value);
    }

    public static TagFloat CreateTag(string name, float value)
    {
      return new TagFloat(name, value);
    }

    public static TagDouble CreateTag(string name, double value)
    {
      return new TagDouble(name, value);
    }

    public static TagString CreateTag(string name, string value)
    {
      return new TagString(name, value);
    }

    public static TagList CreateTag(string name, TagCollection value)
    {
      return new TagList(name, value);
    }

    public static TagCompound CreateTag(string name, TagDictionary value)
    {
      return new TagCompound(name, value);
    }

    public static TagByte CreateTag(byte value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagShort CreateTag(short value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagIntArray CreateTag(int[] value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagByteArray CreateTag(byte[] value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagInt CreateTag(int value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagLong CreateTag(long value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagFloat CreateTag(float value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagDouble CreateTag(double value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagString CreateTag(string value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagList CreateTag(TagCollection value)
    {
      return CreateTag(string.Empty, value);
    }

    public static TagCompound CreateTag(TagDictionary value)
    {
      return CreateTag(string.Empty, value);
    }

    public static ITag CreateTag(TagType tagType, string name, object value)
    {
      ITag result;

      switch (tagType)
      {
        case TagType.Byte:
          result = CreateTag(name, Convert.ToByte(value));
          break;

        case TagType.Short:
          result = CreateTag(name, Convert.ToInt16(value));
          break;

        case TagType.Int:
          result = CreateTag(name, Convert.ToInt32(value));
          break;

        case TagType.Long:
          result = CreateTag(name, Convert.ToInt64(value));
          break;

        case TagType.Float:
          result = CreateTag(name, Convert.ToSingle(value));
          break;

        case TagType.Double:
          result = CreateTag(name, Convert.ToDouble(value));
          break;

        case TagType.ByteArray:
          result = CreateTag(name, (byte[])value);
          break;

        case TagType.String:
          result = CreateTag(Convert.ToString(value));
          break;

        case TagType.List:
          result = CreateTag(name, (TagCollection)value ?? new TagCollection());
          break;

        case TagType.Compound:
          result = CreateTag(name, (TagDictionary)value ?? new TagDictionary());
          break;

        case TagType.IntArray:
          result = CreateTag(name, (int[])value);
          break;

        case TagType.End:
          result = new TagEnd();
          break;

        default:
          throw new ArgumentException($"Unrecognized tag type: {tagType}");
      }

      return result;
    }

    #endregion
  }
}
