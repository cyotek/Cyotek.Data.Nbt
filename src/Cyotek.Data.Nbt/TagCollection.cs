using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public class TagCollection : Collection<Tag>
  {
    #region Fields

    private Tag _owner;

    #endregion

    #region Constructors

    public TagCollection()
    {
      this.LimitType = TagType.None;
    }

    public TagCollection(Tag owner)
      : this(owner, TagType.None)
    { }

    public TagCollection(TagType limitType)
      : this(null, limitType)
    { }

    public TagCollection(Tag owner, TagType limitType)
      : this()
    {
      this.Owner = owner;
      this.LimitType = limitType;
    }

    #endregion

    #region Properties

    public TagType LimitType { get; set; }

    public Tag Owner
    {
      get { return _owner; }
      set
      {
        _owner = value;

        foreach (Tag child in this)
        {
          child.Parent = value;
        }
      }
    }

    #endregion

    #region Methods

    public Tag Add(DateTime value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, DateTime value)
    {
      return this.Add(name, value.ToString("u", CultureInfo.InvariantCulture));
    }

    public Tag Add(string value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, string value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(float value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, float value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(double value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, double value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(long value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, long value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(short value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, short value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(byte value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(bool value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, bool value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, (byte)(value ? 1 : 0));

      this.Add(tag);

      return tag;
    }

    public Tag Add(string name, byte value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(int value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, int value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(int[] value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, int[] value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(byte[] value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, byte[] value)
    {
      Tag tag;

      tag = TagFactory.CreateTag(name, value);

      this.Add(tag);

      return tag;
    }

    public Tag Add(string name, Guid value)
    {
      return this.Add(name, value.ToByteArray());
    }

    public Tag Add(TagType tagType)
    {
      return this.Add(string.Empty, tagType);
    }

    public Tag Add(string name, TagType tagType)
    {
      return this.Add(name, tagType, TagType.None);
    }

    public Tag Add(string name, TagType tagType, TagType limitToType)
    {
      Tag tag;
      ICollectionTag collectionTag;

      tag = TagFactory.CreateTag(tagType);
      tag.Name = name;

      collectionTag = tag as ICollectionTag;
      if (collectionTag != null)
      {
        collectionTag.LimitToType = limitToType;
      }

      this.Add(tag);

      return tag;
    }

    public new void Add(Tag value)
    {
      base.Add(value);
    }

    public Tag Add(object value)
    {
      return this.Add(string.Empty, value);
    }

    public Tag Add(string name, object value)
    {
      Tag result;

      // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
      if (value is byte)
      {
        result = this.Add(name, (byte)value);
      }
      else if (value is byte[])
      {
        result = this.Add(name, (byte[])value);
      }
      else if (value is int)
      {
        result = this.Add(name, (int)value);
      }
      else if (value is int[])
      {
        result = this.Add(name, (int[])value);
      }
      else if (value is float)
      {
        result = this.Add(name, (float)value);
      }
      else if (value is double)
      {
        result = this.Add(name, (double)value);
      }
      else if (value is long)
      {
        result = this.Add(name, (long)value);
      }
      else if (value is short)
      {
        result = this.Add(name, (short)value);
      }
      else if (value is string)
      {
        result = this.Add(name, (string)value);
      }
      else if (value is DateTime)
      {
        result = this.Add(name, (DateTime)value);
      }
      else if (value is Guid)
      {
        result = this.Add(name, (Guid)value);
      }
      else if (value is bool)
      {
        result = this.Add(name, (bool)value);
      }
      else
      {
        throw new ArgumentException("Invalid value type.", nameof(value));
      }
      // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull

      return result;
    }

    public void AddRange(IEnumerable<object> values)
    {
      foreach (object value in values)
      {
        this.Add(string.Empty, value);
      }
    }

    public void AddRange(IEnumerable<KeyValuePair<string, object>> values)
    {
      foreach (KeyValuePair<string, object> value in values)
      {
        this.Add(value.Key, value.Value);
      }
    }

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>
    /// A string that represents the current object.
    /// </returns>
    public override string ToString()
    {
      StringBuilder sb;

      sb = new StringBuilder();

      sb.Append('[');

      foreach (Tag tag in this)
      {
        if (sb.Length > 1)
        {
          sb.Append(',').Append(' ');
        }

        sb.Append(tag.ToValueString());
      }

      sb.Append(']');

      return sb.ToString();
    }

    protected override void ClearItems()
    {
      foreach (Tag item in this)
      {
        item.Parent = null;
      }

      base.ClearItems();
    }

    protected override void InsertItem(int index, Tag item)
    {
      if (this.LimitType != TagType.None && item.Type != this.LimitType)
      {
        throw new ArgumentException($"Only items of type {this.LimitType} can be added to this collection.", nameof(item));
      }

      item.Parent = this.Owner;

      base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
      Tag item;

      item = this[index];
      item.Parent = null;

      base.RemoveItem(index);
    }

    protected override void SetItem(int index, Tag item)
    {
      if (this.LimitType != TagType.None && item.Type != this.LimitType)
      {
        throw new ArgumentException($"Only items of type {this.LimitType} can be added to this collection.", nameof(item));
      }

      item.Parent = this.Owner;

      base.SetItem(index, item);
    }

    #endregion
  }
}
