using System;
using System.Collections.ObjectModel;

namespace Cyotek.Data.Nbt
{
  public class TagCollection
    : Collection<ITag>
  {
    public TagCollection(ITag owner)
      : this(owner, TagType.None)
    { }

    public TagCollection(ITag owner, TagType limitType)
      : this()
    {
      this.Owner = owner;
      this.LimitType = limitType;
    }

    public TagCollection()
    {
      this.LimitType = TagType.None;
    }

    public TagCollection(TagType limitType)
      : this(null, limitType)
    { }

    public TagType LimitType { get; set; }

    public ITag Owner { get; set; }

    public ITag Add(string name, DateTime value)
    {
      return this.Add(name, value.ToString("u"));
    }

    public ITag Add(string name, string value)
    {
      ITag tag;

      tag = new TagString(name, value);

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

    protected override void ClearItems()
    {
      foreach (ITag item in this)
        item.Parent = null;

      base.ClearItems();
    }

    protected override void InsertItem(int index, ITag item)
    {
      if (this.LimitType != TagType.None && item.Type != this.LimitType)
        throw new ArgumentException(string.Format("Only items of type {0} can be added to this collection.", this.LimitType), "item");

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
        throw new ArgumentException(string.Format("Only items of type {0} can be added to this collection.", this.LimitType), "item");

      item.Parent = this.Owner;

      base.SetItem(index, item);
    }
  }
}