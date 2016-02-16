using System;
using System.Collections.Generic;

namespace Cyotek.Data.Nbt
{
  public class TagList : Tag, ICollectionTag
  {
    #region Constructors

    public TagList()
    {
      this.Name = string.Empty;
      this.Value = new TagCollection(this);
      this.ListType = TagType.None;
    }

    public TagList(string name)
      : this(name, TagType.None)
    { }

    public TagList(TagType listType)
      : this(string.Empty, listType)
    { }

    public TagList(string name, TagType listType)
      : this()
    {
      this.Name = name;
      this.ListType = listType;
    }

    #endregion

    #region Properties

    public int Count
    {
      get { return this.Value.Count; }
    }

    public virtual TagType ListType
    {
      get { return this.Value?.LimitType ?? TagType.None; }
      set
      {
        if (this.Value == null || this.Value.LimitType != value)
        {
          this.Value = new TagCollection(this, value);
        }
      }
    }

    public new TagCollection Value
    {
      get { return (TagCollection)base.Value; }
      set
      {
        if (value == null)
        {
          throw new ArgumentNullException(nameof(value));
        }

        base.Value = value;
        value.Owner = this;
      }
    }

    #endregion

    #region ICollectionTag Interface

    public override string ToString(string indentString)
    {
      return $"{indentString}[List: {this.Name}] ({this.Value?.Count ?? 0} items)";
    }

    public override TagType Type
    {
      get { return TagType.List; }
    }

    bool ICollectionTag.IsList
    {
      get { return true; }
    }

    TagType ICollectionTag.LimitToType
    {
      get { return this.ListType; }
      set { this.ListType = value; }
    }

    IList<ITag> ICollectionTag.Values
    {
      get { return this.Value; }
    }

    #endregion
  }
}
