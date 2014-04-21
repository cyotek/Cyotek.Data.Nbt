using System;
using System.Collections.Generic;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.NtbNullEditor, Cyotek.Windows.Forms.Nbt")]
  public class TagList : Tag, ICollectionTag
  {
    #region Public Constructors

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

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.List; }
    }

    #endregion

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[List: {1}] ({2} items)", indentString, this.Name, this.Value != null ? this.Value.Count : 0);
    }

    #endregion

    #region Public Properties

    public int Count
    {
      get { return this.Value.Count; }
    }

    public virtual TagType ListType
    {
      get { return this.Value == null ? TagType.None : this.Value.LimitType; }
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
          throw new ArgumentNullException("value");
        }

        base.Value = value;
        value.Owner = this;
      }
    }

    #endregion

    #region ICollectionTag Members

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
