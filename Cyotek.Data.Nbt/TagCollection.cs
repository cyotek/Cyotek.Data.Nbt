using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public class TagCollection : Collection<ITag>
  {
    #region Public Constructors

    public TagCollection()
    {
      this.LimitType = TagType.None;
    }

    public TagCollection(ITag owner)
      : this(owner, TagType.None)
    { }

    public TagCollection(TagType limitType)
      : this(null, limitType)
    { }

    public TagCollection(ITag owner, TagType limitType)
      : this()
    {
      this.Owner = owner;
      this.LimitType = limitType;
    }

    #endregion

    #region Overridden Methods

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

      foreach (ITag tag in this)
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
      foreach (ITag item in this)
      {
        item.Parent = null;
      }

      base.ClearItems();
    }

    protected override void InsertItem(int index, ITag item)
    {
      if (this.LimitType != TagType.None && item.Type != this.LimitType)
      {
        throw new ArgumentException(string.Format("Only items of type {0} can be added to this collection.", this.LimitType), "item");
      }

      item.Parent = this.Owner;

      base.InsertItem(index, item);
    }

    protected override void RemoveItem(int index)
    {
      ITag item;

      item = this[index];
      item.Parent = null;

      base.RemoveItem(index);
    }

    protected override void SetItem(int index, ITag item)
    {
      if (this.LimitType != TagType.None && item.Type != this.LimitType)
      {
        throw new ArgumentException(string.Format("Only items of type {0} can be added to this collection.", this.LimitType), "item");
      }

      item.Parent = this.Owner;

      base.SetItem(index, item);
    }

    #endregion

    #region Public Properties

    public TagType LimitType { get; set; }

    public ITag Owner { get; set; }

    #endregion

    #region Public Members

    public ITag Add(DateTime value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, DateTime value)
    {
      return this.Add(name, value.ToString("u"));
    }

    public ITag Add(string value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, string value)
    {
      ITag tag;

      tag = new TagString(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(float value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, float value)
    {
      ITag tag;

      tag = new TagFloat(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(double value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, double value)
    {
      ITag tag;

      tag = new TagDouble(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(long value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, long value)
    {
      ITag tag;

      tag = new TagLong(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(short value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, short value)
    {
      ITag tag;

      tag = new TagShort(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(byte value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(bool value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, bool value)
    {
      ITag tag;

      tag = new TagByte(name, (byte)(value ? 1 : 0));

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, byte value)
    {
      ITag tag;

      tag = new TagByte(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(int value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, int value)
    {
      ITag tag;

      tag = new TagInt(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(int[] value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, int[] value)
    {
      ITag tag;

      tag = new TagIntArray(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(byte[] value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, byte[] value)
    {
      ITag tag;

      tag = new TagByteArray(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, Guid value)
    {
      return this.Add(name, value.ToByteArray());
    }

    public ITag Add(TagType tagType)
    {
      return this.Add(string.Empty, tagType);
    }

    public ITag Add(string name, TagType tagType)
    {
      return this.Add(name, tagType, TagType.None);
    }

    public ITag Add(string name, TagType tagType, TagType limitToType)
    {
      ITag tag;
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

    public new void Add(ITag value)
    {
      base.Add(value);
    }

    public ITag Add(object value)
    {
      return this.Add(string.Empty, value);
    }

    public ITag Add(string name, object value)
    {
      ITag result;

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
        throw new ArgumentException("Invalid value type.", "value");
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

    #endregion
  }
}
