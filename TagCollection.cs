using System;
using System.Collections.ObjectModel;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public class TagCollection : Collection<ITag>
  {
    #region  Public Constructors

    public TagCollection(ITag owner)
      : this(owner, TagType.None)
    { }

    public TagCollection(ITag owner, TagType limitType)
      : this()
    {
      if (owner == null)
        throw new ArgumentNullException("owner");

      this.Owner = owner;
      this.LimitType = limitType;
    }

    #endregion  Public Constructors

    #region  Protected Constructors

    protected TagCollection()
    { }

    #endregion  Protected Constructors

    #region  Protected Overridden Methods

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

    #endregion  Protected Overridden Methods

    #region  Public Methods

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

    public void WriteList(Stream output)
    {
      if (this.LimitType == TagType.None || this.LimitType == TagType.End)
        throw new InvalidOperationException("Limit type not set.");

      output.WriteByte((byte)this.LimitType);

      TagInt.WriteInt(output, this.Count);

      foreach (ITag item in this)
        item.WriteUnnamed(output);
    }

    #endregion  Public Methods

    #region  Public Properties

    public TagType LimitType { get; protected set; }

    public ITag Owner { get; protected set; }

    #endregion  Public Properties
  }
}
