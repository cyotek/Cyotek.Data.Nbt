using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagCompound : Tag, ICollectionTag, IEquatable<TagCompound>
  {
    #region Constants

    private static readonly char[] _queryDelimiters =
    {
      '\\',
      '/'
    };

    #endregion

    #region Fields

    private TagDictionary _value;

    #endregion

    #region Constructors

    public TagCompound()
      : this(string.Empty)
    { }

    public TagCompound(string name)
      : this(name, new TagDictionary())
    { }

    public TagCompound(TagDictionary value)
      : this(string.Empty, value)
    { }

    public TagCompound(string name, TagDictionary value)
      : base(name)
    {
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Compound; }
    }

    [Category("Data")]
    [DefaultValue(typeof(TagDictionary), null)]
    public TagDictionary Value
    {
      get { return _value; }
      set
      {
        if (!ReferenceEquals(_value, value))
        {
          if (value == null)
          {
            throw new ArgumentNullException(nameof(value));
          }

          _value = value;
          value.Owner = this;
        }
      }
    }

    #endregion

    #region Methods

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

      return value?.Value ?? defaultValue;
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

      return value != null ? DateTime.Parse(value.Value, CultureInfo.InvariantCulture).ToUniversalTime() : defaultValue;
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

      return value?.Value ?? defaultValue;
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

      return value?.Value ?? defaultValue;
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

    public override int GetHashCode()
    {
      // http://stackoverflow.com/a/263416/148962

      unchecked // Overflow is fine, just wrap
      {
        int hash;
        TagDictionary values;

        hash = 17;
        hash = hash * 23 + this.Name.GetHashCode();

        values = this.Value;

        if (values != null)
        {
          for (int i = 0; i < values.Count; i++)
          {
            hash = hash * 23 + _value[i].GetHashCode();
          }
        }

        return hash;
      }
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

      return value?.Value ?? defaultValue;
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

      return value?.Value ?? defaultValue;
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

      return value?.Value ?? defaultValue;
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

    public T GetTag<T>(string name) where T : Tag
    {
      Tag value;

      this.Value.TryGetValue(name, out value);

      return (T)value;
    }

    public Tag GetTag(string name)
    {
      return this.GetTag<Tag>(name);
    }

    public override object GetValue()
    {
      return _value;
    }

    public Tag Query(string query)
    {
      return this.Query<Tag>(query);
    }

    public T Query<T>(string query) where T : Tag
    {
      string[] parts;
      Tag element;

      parts = query.Split(_queryDelimiters);
      element = this;

      // HACK: This is all quickly thrown together

      foreach (string part in parts)
      {
        if (part.IndexOf('[') != -1)
        {
          string[] subParts;
          string name;
          string value;
          bool matchFound;
          TagList list;

          subParts = part.Substring(1, part.Length - 2).Split('=');
          name = subParts[0];
          value = subParts[1];
          matchFound = false;

          list = element as TagList;

          if (list != null)
          {
            // ReSharper disable once LoopCanBePartlyConvertedToQuery
            foreach (Tag tag in list.Value)
            {
              TagCompound compound;

              compound = tag as TagCompound;

              if (compound != null && compound.GetStringValue(name) == value)
              {
                element = tag;
                matchFound = true;
                break;
              }
            }
          }

          if (!matchFound)
          {
            throw new ArgumentException($"Could not find element matching pattern '{part}'", nameof(query));
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
      Tag tag;

      tag = this.Query<Tag>(query);

      return tag != null ? (T)tag.GetValue() : defaultValue;
    }

    public override void SetValue(object value)
    {
      this.Value = (TagDictionary)value;
    }

    public override string ToString()
    {
      int count;

      count = _value?.Count ?? 0;

      return string.Concat("[", this.Type, ": ", this.Name, "] (", count.ToString(CultureInfo.InvariantCulture), " items)");
    }

    public override string ToValueString()
    {
      return _value?.ToString() ?? string.Empty;
    }

    #endregion

    #region ICollectionTag Interface

    bool ICollectionTag.IsList
    {
      get { return false; }
    }

    TagType ICollectionTag.LimitToType
    {
      get { return TagType.None; }
      set { }
    }

    IList<Tag> ICollectionTag.Values
    {
      get { return this.Value; }
    }

    #endregion

    #region IEquatable<TagCompound> Interface

    public bool Equals(TagCompound other)
    {
      bool result;

      result = !ReferenceEquals(null, other);

      if (result && !ReferenceEquals(this, other))
      {
        result = string.Equals(this.Name, other.Name);

        if (result)
        {
          IList<Tag> src;
          IList<Tag> dst;

          src = this.Value;
          dst = other.Value;

          result = src.Count == dst.Count;

          for (int i = 0; i < src.Count; i++)
          {
            Tag srcTag;
            Tag dstTag;

            srcTag = src[i];
            dstTag = dst[i];

            if (!srcTag.Equals(dstTag))
            {
              result = false;
              break;
            }
          }
        }
      }

      return result;
    }

    #endregion
  }
}
