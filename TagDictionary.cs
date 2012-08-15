using System;
using System.Collections.ObjectModel;

namespace Cyotek.Data.Nbt
{
  public class TagDictionary : KeyedCollection<string, ITag>
  {
    #region  Public Constructors

    public TagDictionary(ITag owner)
      : this()
    {
      if (owner == null)
        throw new ArgumentNullException("owner");

      this.Owner = owner;
    }

    #endregion  Public Constructors

    #region  Protected Constructors

    protected TagDictionary()
    { }

    #endregion  Protected Constructors

    #region  Protected Overridden Methods

    protected override void ClearItems()
    {
      foreach (ITag item in this)
        item.Parent = null;

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

    #endregion  Protected Overridden Methods

    #region  Public Methods

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

    public void Add(string name, Guid value)
    {
      this.Add(name, value.ToByteArray());
    }

    public ITag Add(TagType tagType)
    {
      return this.Add(tagType, string.Empty);
    }

    public ITag Add(TagType tagType, string name)
    {
      return this.Add(tagType, name, TagType.None);
    }

    public ITag Add(TagType tagType, string name, TagType limitToType)
    {
      ITag tag;

      tag = TagFactory.CreateTag(tagType);
      tag.Name = name;
      if (tag is ICollectionTag)
        ((ICollectionTag)tag).LimitToType = limitToType;

      this.Add(tag);

      return tag;
    }

    public ITag AddIfNotDefault(string name, string value)
    {
      ITag result;

      if (!string.IsNullOrEmpty(value))
        result = this.Add(name, value);
      else
        result = null;

      return result;
    }

    public ITag AddIfNotDefault(string name, bool value)
    {
      ITag result;

      if (value)
        result = this.Add(name, value);
      else
        result = null;

      return result;
    }

    public ITag AddIfNotDefault(string name, long value)
    {
      ITag result;

      if (value != 0)
        result = this.Add(name, value);
      else
        result = null;

      return result;
    }

    public ITag AddIfNotDefault(string name, short value)
    {
      ITag result;

      if (value != 0)
        result = this.Add(name, value);
      else
        result = null;

      return result;
    }

    public ITag AddIfNotDefault(string name, double value)
    {
      ITag result;

      if (value != 0)
        result = this.Add(name, value);
      else
        result = null;

      return result;
    }

    public ITag AddIfNotDefault(string name, int value)
    {
      ITag result;

      if (value != 0)
        result = this.Add(name, value);
      else
        result = null;

      return result;
    }

    public ITag AddIfNotDefault(string name, float value)
    {
      ITag result;

      if (value != 0)
        result = this.Add(name, value);
      else
        result = null;

      return result;
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

    #endregion  Public Methods

    #region  Internal Methods

    internal void ChangeKey(ITag item, string newKey)
    {
      base.ChangeItemKey(item, newKey);
    }

    #endregion  Internal Methods

    #region  Public Properties

    public ITag Owner { get; protected set; }

    #endregion  Public Properties
  }
}
