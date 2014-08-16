using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public class TagDictionary : KeyedCollection<string, ITag>
  {
    #region Public Constructors

    public TagDictionary()
    { }

    public TagDictionary(ITag owner)
      : this()
    {
      if (owner == null)
      {
        throw new ArgumentNullException("owner");
      }

      this.Owner = owner;
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

    protected override string GetKeyForItem(ITag item)
    {
      return item.Name;
    }

    protected override void InsertItem(int index, ITag item)
    {
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
      item.Parent = this.Owner;

      base.SetItem(index, item);
    }

    #endregion

    #region Public Properties

    public ITag Owner { get; set; }

    #endregion

    #region Public Members

    public ITag Add(string name, string value)
    {
      ITag tag;

      tag = new TagString(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, bool value)
    {
      ITag tag;

      tag = new TagByte(name, (byte)(value ? 1 : 0));

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, float value)
    {
      ITag tag;

      tag = new TagFloat(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, double value)
    {
      ITag tag;

      tag = new TagDouble(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, long value)
    {
      ITag tag;

      tag = new TagLong(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, short value)
    {
      ITag tag;

      tag = new TagShort(name, value);

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

    public ITag Add(string name, int value)
    {
      ITag tag;

      tag = new TagInt(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, int[] value)
    {
      ITag tag;

      tag = new TagIntArray(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, byte[] value)
    {
      ITag tag;

      tag = new TagByteArray(name, value);

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, DateTime value)
    {
      return this.Add(name, value.ToString("u"));
    }

    public ITag Add(string name, Guid value)
    {
      return this.Add(name, value.ToByteArray());
    }

    public ITag Add(string name, TagType tagType)
    {
      return this.Add(name, tagType, TagType.None);
    }

    public ITag Add(string name, TagType tagType, object value)
    {
      ITag tag;

      tag = this.Add(name, tagType);
      tag.Value = value;

      return tag;
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

    public void AddRange(IEnumerable<KeyValuePair<string, object>> values)
    {
      foreach (KeyValuePair<string, object> value in values)
      {
        this.Add(value.Key, value.Value);
      }
    }

    public bool TryGetValue(string key, out ITag value)
    {
      bool result;

      if (this.Dictionary != null)
      {
        result = this.Dictionary.TryGetValue(key, out value);
      }
      else
      {
        result = false;
        value = null;
      }

      return result;
    }

    #endregion

    #region Internal Members

    internal void ChangeKey(ITag item, string newKey)
    {
      base.ChangeItemKey(item, newKey);
    }

    #endregion
  }
}
