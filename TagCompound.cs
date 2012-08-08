using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.NtbNullEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagCompound : Tag, ICollectionTag
  {
    #region  Public Constructors

    public TagCompound()
      : this(string.Empty)
    { }

    public TagCompound(string name)
    {
      this.Name = name;
      this.Value = new TagDictionary(this);
    }

    public TagCompound(Stream input)
    {
      this.Name = TagString.ReadString(input);
      this.Value = ReadDictionary(this, input);
    }

    #endregion  Public Constructors

    #region  Private Class Methods

    private static void WriteCompound(Stream output, TagCompound tagCompound)
    {
      foreach (ITag item in tagCompound.Value)
        item.Write(output);

      TagEnd.Singleton.Write(output);
    }

    #endregion  Private Class Methods

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Compound; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static TagDictionary ReadDictionary(TagCompound owner, Stream input)
    {
      TagDictionary results;
      ITag tag;

      results = new TagDictionary(owner);

      tag = Tag.Read(input);
      while (tag.Type != TagType.End)
      {
        results.Add(tag);
        tag = Tag.Read(input);
      }

      return results;
    }

    internal static TagCompound ReadUnnamedTagCompound(Stream input)
    {
      TagCompound result;

      result = new TagCompound();
      result.Value = ReadDictionary(result, input);

      return result;
    }

    #endregion  Internal Class Methods

    #region  Public Overridden Methods

    public override string ToString(string indentString)
    {
      StringBuilder results;

      results = new StringBuilder();

      if (this.Value.Count == 0)
        results.AppendFormat("{0}[Compound: {1}]", indentString, this.Name);
      else
      {
        results.AppendLine(string.Format("{0}[Compound: {1}", indentString, this.Name));

        foreach (ITag item in this.Value)
          results.AppendLine(string.Format("{0}  {1}={2}", indentString, item.Name, item.ToString(indentString + "  ").Trim()));

        results.AppendLine(string.Format("{0}]", indentString));
      }

      return results.ToString();
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagCompound.WriteCompound(output, this);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteCompound(output, this);
    }

    #endregion  Public Overridden Methods

    #region  Public Methods

    public bool Contains(string name)
    {
      return this.Value.Contains(name);
    }

    public bool GetBoolValue(string name)
    {
      return this.GetBoolValue(name, false);
    }

    public bool GetBoolValue(string name, bool defaultValue)
    {
      TagByte value;

      value = this.GetTag<TagByte>(name);

      return value != null ? value.Value != 0 ? true : false : defaultValue;
    }

    public TagByte GetByte(string name)
    {
      return this.GetTag<TagByte>(name);
    }

    public TagByteArray GetByteArray(string name)
    {
      return this.GetTag<TagByteArray>(name);
    }

    public byte[] GetByteArrayValue(string name)
    {
      return this.GetByteArrayValue(name, new byte[0]);
    }

    public byte[] GetByteArrayValue(string name, byte[] defaultValue)
    {
      TagByteArray value;

      value = this.GetTag<TagByteArray>(name);

      return value != null ? value.Value : defaultValue;
    }

    public TagCompound GetCompound(string name)
    {
      return this.GetTag<TagCompound>(name);
    }

    public TagDouble GetDouble(string name)
    {
      return this.GetTag<TagDouble>(name);
    }

    public double GetDoubleValue(string name)
    {
      return this.GetDoubleValue(name, 0);
    }

    public double GetDoubleValue(string name, double defaultValue)
    {
      TagDouble value;

      value = this.GetTag<TagDouble>(name);

      return value != null ? value.Value : defaultValue;
    }

    public T GetEnumValue<T>(string name) where T : struct
    {
      return this.GetEnumValue<T>(name, default(T));
    }

    public T GetEnumValue<T>(string name, T defaultValue) where T : struct
    {
      TagInt value;

      value = this.GetTag<TagInt>(name);

      return value != null ? (T)Enum.ToObject(typeof(T), value.Value) : defaultValue;
    }

    public TagFloat GetFloat(string name)
    {
      return this.GetTag<TagFloat>(name);
    }

    public float GetFloatValue(string name)
    {
      return this.GetFloatValue(name, 0);
    }

    public float GetFloatValue(string name, float defaultValue)
    {
      TagFloat value;

      value = this.GetTag<TagFloat>(name);

      return value != null ? value.Value : defaultValue;
    }

    public TagInt GetInt(string name)
    {
      return this.GetTag<TagInt>(name);
    }

    public TagIntArray GetIntArray(string name)
    {
      return this.GetTag<TagIntArray>(name);
    }

    public int[] GetIntArrayValue(string name)
    {
      return this.GetIntArrayValue(name, new int[0]);
    }

    public int[] GetIntArrayValue(string name, int[] defaultValue)
    {
      TagIntArray value;

      value = this.GetTag<TagIntArray>(name);

      return value != null ? value.Value : defaultValue;
    }

    public int GetIntValue(string name)
    {
      return this.GetIntValue(name, 0);
    }

    public int GetIntValue(string name, int defaultValue)
    {
      TagInt value;

      value = this.GetTag<TagInt>(name);

      return value != null ? value.Value : defaultValue;
    }

    public TagList GetList(string name)
    {
      return this.GetTag<TagList>(name);
    }

    public TagLong GetLong(string name)
    {
      return this.GetTag<TagLong>(name);
    }

    public long GetLongValue(string name)
    {
      return this.GetLongValue(name, 0);
    }

    public long GetLongValue(string name, long defaultValue)
    {
      TagLong value;

      value = this.GetTag<TagLong>(name);

      return value != null ? value.Value : defaultValue;
    }

    public TagShort GetShort(string name)
    {
      return this.GetTag<TagShort>(name);
    }

    public short GetShortValue(string name)
    {
      return this.GetShortValue(name, 0);
    }

    public short GetShortValue(string name, short defaultValue)
    {
      TagShort value;

      value = this.GetTag<TagShort>(name);

      return value != null ? value.Value : defaultValue;
    }

    public TagString GetString(string name)
    {
      return this.GetTag<TagString>(name);
    }

    public string GetStringValue(string name)
    {
      return this.GetStringValue(name, null);
    }

    public string GetStringValue(string name, string defaultValue)
    {
      TagString value;

      value = this.GetTag<TagString>(name);

      return value != null ? value.Value : defaultValue;
    }

    public T GetTag<T>(string name) where T : ITag
    {
      ITag value;

      this.Value.TryGetValue(name, out value);

      return (T)value;
    }

    public ITag GetTag(string name)
    {
      return this.GetTag<ITag>(name);
    }

    public ITag Query(string query)
    {
      return this.Query<ITag>(query);
    }

    public T Query<T>(string query) where T : ITag
    {
      string[] parts;
      ITag element;

      parts = query.Split(new char[] { '\\', '/' });
      element = this;

      // HACK: This is all quickly thrown together

      foreach (string part in parts)
      {
        if (part.Contains("["))
        {
          string[] subParts;
          string name;
          string value;
          bool matchFound;

          subParts = part.Substring(1, part.Length - 2).Split('=');
          name = subParts[0];
          value = subParts[1];
          matchFound = false;

          foreach (TagCompound tag in ((TagList)element).Value)
          {
            if (tag.GetStringValue(name) == value)
            {
              element = tag;
              matchFound = true;
              break;
            }
          }

          if (!matchFound)
            throw new ArgumentException(string.Format("Could not find element matching pattern '{0}'", part), "query");
        }
        else if (element is ICollectionTag && ((ICollectionTag)element).IsList)
        {
          // list entry
          element = ((ICollectionTag)element).Values[Convert.ToInt32(part)];
        }
        else
        {
          // standard item
          element = ((TagCompound)element).Value[part];
        }
      }

      return (T)element;
    }

    public T QueryValue<T>(string query)
    {
      return this.QueryValue<T>(query, default(T));
    }

    public T QueryValue<T>(string query, T defaultValue)
    {
      ITag tag;

      tag = this.Query<ITag>(query);

      return tag != null ? (T)tag.Value : defaultValue;
    }

    public void WriteToFile(string filename)
    {
      using (FileStream output = File.Open(filename, FileMode.Create))
      {
        using (GZipStream gzipStream = new GZipStream(output, CompressionMode.Compress))
        {
          Write(gzipStream);
        }
      }
    }

    #endregion  Public Methods

    #region  Public Properties

    public new TagDictionary Value
    {
      get { return (TagDictionary)base.Value; }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");

        base.Value = value;
      }
    }

    #endregion  Public Properties

    #region  Private Properties

    bool ICollectionTag.IsList
    { get { return false; } }

    TagType ICollectionTag.LimitToType
    { get { return TagType.None; } }

    IList<ITag> ICollectionTag.Values
    {
      get { return this.Value; }
    }

    #endregion  Private Properties
  }
}
