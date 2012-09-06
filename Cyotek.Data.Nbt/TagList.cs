﻿using System;
using System.Collections.Generic;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.NtbNullEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagList
    : Tag, ICollectionTag
  {
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

    bool ICollectionTag.IsList
    { get { return true; } }

    TagType ICollectionTag.LimitToType
    {
      get { return this.ListType; }
      set { this.ListType = value; }
    }

    IList<ITag> ICollectionTag.Values
    {
      get { return this.Value; }
    }

    public int Count
    { get { return this.Value.Count; } }

    public virtual TagType ListType
    {
      get { return this.Value == null ? TagType.None : this.Value.LimitType; }
      set
      {
        if (this.Value == null || this.Value.LimitType != value)
          this.Value = new TagCollection(this, value);
      }
    }

    public override TagType Type
    {
      get { return TagType.List; }
    }

    public new TagCollection Value
    {
      get { return (TagCollection)base.Value; }
      set
      {
        if (value == null)
          throw new ArgumentNullException("value");

        base.Value = value;
        value.Owner = this;
      }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[List: {1}] ({2} items)", indentString, this.Name, this.Value != null ? this.Value.Count : 0);
    }
  }
}