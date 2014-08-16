using System;
using System.Collections.Generic;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.NtbNullEditor, Cyotek.Windows.Forms.Nbt")]
  public class TagCompound : Tag, ICollectionTag
  {
    #region Public Constructors

    public TagCompound()
      : this(string.Empty)
    { }

    public TagCompound(string name)
    {
      this.Name = name;
      this.Value = new TagDictionary(this);
    }

    #endregion

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.Compound; }
    }

    #endregion

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Compound: {1}] ({2} entries)", indentString, this.Name, this.Value != null ? this.Value.Count : 0);
    }

    #endregion

    #region Public Properties

    public new TagDictionary Value
    {
      get { return (TagDictionary)base.Value; }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException("value");
        }

        base.Value = value;
        value.Owner = this;
      }
    }

    #endregion

    #region Public Members

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

      return value != null ? value.Value != 0 : defaultValue;
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

    public byte GetByteValue(string name)
    {
      return this.GetByteValue(name, default(byte));
    }

    public byte GetByteValue(string name, byte defaultValue)
    {
      TagByte value;

      value = this.GetTag<TagByte>(name);

      return value != null ? value.Value : defaultValue;
    }

    public TagCompound GetCompound(string name)
    {
      return this.GetTag<TagCompound>(name);
    }

    public DateTime GetDateTimeValue(string name)
    {
      return this.GetDateTimeValue(name, DateTime.MinValue);
    }

    public DateTime GetDateTimeValue(string name, DateTime defaultValue)
    {
      TagString value;

      value = this.GetTag<TagString>(name);

      return value != null ? DateTime.Parse(value.Value).ToUniversalTime() : defaultValue;
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
      return this.GetEnumValue(name, default(T));
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

    public Guid GetGuidValue(string name)
    {
      return this.GetGuidValue(name, Guid.Empty);
    }

    public Guid GetGuidValue(string name, Guid defaultValue)
    {
      TagByteArray tag;

      tag = this.GetByteArray(name);

      return tag != null ? new Guid(tag.Value) : defaultValue;
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

      parts = query.Split(new[]
                          {
                            '\\', '/'
                          });
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

          if (element is TagList)
          {
            foreach (TagCompound tag in ((TagList)element).Value)
            {
              if (tag.GetStringValue(name) == value)
              {
                element = tag;
                matchFound = true;
                break;
              }
            }
          }

          if (!matchFound)
          {
            throw new ArgumentException(string.Format("Could not find element matching pattern '{0}'", part), "query");
          }
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
      return this.QueryValue(query, default(T));
    }

    public T QueryValue<T>(string query, T defaultValue)
    {
      ITag tag;

      tag = this.Query<ITag>(query);

      return tag != null ? (T)tag.Value : defaultValue;
    }

    #endregion

    #region ICollectionTag Members

    bool ICollectionTag.IsList
    {
      get { return false; }
    }

    TagType ICollectionTag.LimitToType
    {
      get { return TagType.None; }
      set { }
    }

    IList<ITag> ICollectionTag.Values
    {
      get { return this.Value; }
    }

    #endregion
  }
}
