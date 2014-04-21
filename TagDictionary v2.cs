using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Cyotek.Data.Nbt
{
  public class TagDictionary : IList<ITag>
  {
    #region Constructors

    protected IDictionary<string, ITag> Dictionary { get; set; }

    public TagDictionary()
    {
      this.Dictionary = new Dictionary<string, ITag>();
    }

    public TagDictionary(ITag owner)
      : this()
    {
      if (owner == null)
        throw new ArgumentNullException("owner");

      this.Owner = owner;
    }

    #endregion

    #region Overridden Members

    public ITag this[string name]
    {
      get { return this.Dictionary[name]; }
      set
      {
        this.Remove(value.Name);
        this.Add(value);
      }
    }

    public void Clear()
    {
      this.Dictionary.Clear();
    }

    public bool Remove(string name)
    {
      ITag item;

      if (this.TryGetValue(name, out item))
        item.Parent = null;

      return this.Dictionary.Remove(name);
    }

    public bool Remove(ITag tag)
    {
      return this.Remove(tag.Name);
    }

    #endregion

    #region Properties

    public ITag Owner { get; set; }

    #endregion

    #region Members

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

    public void Add(ITag tag)
    {
      tag.Parent = this.Owner;

      this.Dictionary.Add(tag.Name, tag);
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
        collectionTag.LimitToType = limitToType;

      this.Add(tag);

      return tag;
    }

    public ITag Add(string name, object value)
    {
      ITag result;

      // ReSharper disable CanBeReplacedWithTryCastAndCheckForNull
      if (value is byte)
        result = this.Add(name, (byte)value);
      else if (value is byte[])
        result = this.Add(name, (byte[])value);
      else if (value is int)
        result = this.Add(name, (int)value);
      else if (value is int[])
        result = this.Add(name, (int[])value);
      else if (value is float)
        result = this.Add(name, (float)value);
      else if (value is double)
        result = this.Add(name, (double)value);
      else if (value is long)
        result = this.Add(name, (long)value);
      else if (value is short)
        result = this.Add(name, (short)value);
      else if (value is string)
        result = this.Add(name, (string)value);
      else if (value is DateTime)
        result = this.Add(name, (DateTime)value);
      else if (value is Guid)
        result = this.Add(name, (Guid)value);
      else if (value is bool)
        result = this.Add(name, (bool)value);
      else
        throw new ArgumentException("Invalid value type.", "value");
      // ReSharper restore CanBeReplacedWithTryCastAndCheckForNull

      return result;
    }

    public void AddRange(IEnumerable<KeyValuePair<string, object>> values)
    {
      foreach (KeyValuePair<string, object> value in values)
        this.Add(value.Key, value.Value);
    }

    public bool TryGetValue(string key, out ITag value)
    {
      bool result;

      if (this.Dictionary != null)
        result = this.Dictionary.TryGetValue(key, out value);
      else
      {
        result = false;
        value = null;
      }

      return result;
    }

    #endregion


    public IEnumerator<ITag> GetEnumerator()
    {
      return this.Dictionary.Values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }

    public int IndexOf(ITag item)
    {
      throw new NotImplementedException();
    }

    public void Insert(int index, ITag item)
    {
      throw new NotImplementedException();
    }

    public void RemoveAt(int index)
    {
      throw new NotImplementedException();
    }

    public IEnumerable<string> Keys
    { get { return this.Keys; } }

    public ITag this[int index]
    {
      get
      {
        throw new NotImplementedException();
      }
      set
      {
        throw new NotImplementedException();
      }
    }

    public bool Contains(string name)
    {
      return this.Dictionary.ContainsKey(name);
    }

    public bool Contains(ITag item)
    {
      return this.Dictionary.ContainsKey(item.Name);
    }

    public void CopyTo(ITag[] array, int arrayIndex)
    {
      this.Dictionary.Values.CopyTo(array, arrayIndex);
    }

    public int Count
    {
      get { return this.Dictionary.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }
  }
}
