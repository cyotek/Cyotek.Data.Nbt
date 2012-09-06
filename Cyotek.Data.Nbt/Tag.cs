using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public abstract class Tag
    : ITag
  {
    private string _name;
    private ITag _parent;
    private object _value;

    [Category("Property Changed")]
    public event EventHandler NameChanged;

    [Category("Property Changed")]
    public event EventHandler ParentChanged;

    [Category("Property Changed")]
    public event EventHandler ValueChanged;

    public virtual bool CanRemove
    { get { return this.Parent != null && this.Parent is ICollectionTag; } }

    public virtual string FullPath
    {
      get
      {
        StringBuilder results;

        results = new StringBuilder();

        if (this.Parent != null)
        {
          results.Append(this.Parent.FullPath);
          results.Append(@"\");
        }

        if (this.Parent is ICollectionTag && ((ICollectionTag)this.Parent).IsList)
          results.Append(((ICollectionTag)this.Parent).Values.IndexOf(this));
        else
          results.Append(this.Name);

        return results.ToString();
      }
    }

    [Category(""), DefaultValue("")]
    public virtual string Name
    {
      get { return _name; }
      set
      {
        if (this.Name != value)
        {
          if (this.Parent != null && this.Parent is ICollectionTag && ((ICollectionTag)this.Parent).Values is TagDictionary)
            ((TagDictionary)((ICollectionTag)this.Parent).Values).ChangeKey(this, value);

          _name = value;

          this.OnNameChanged(EventArgs.Empty);
        }
      }
    }

    [Browsable(false)]
    public virtual ITag Parent
    {
      get { return _parent; }
      set
      {
        if (this.Parent != value)
        {
          _parent = value;

          this.OnParentChanged(EventArgs.Empty);
        }
      }
    }

    public abstract TagType Type { get; }

    [Category(""), DefaultValue(null)]
    public object Value
    {
      get { return _value; }
      set
      {
        if (this.Value != value)
        {
          _value = value;

          this.OnValueChanged(EventArgs.Empty);
        }
      }
    }

    public ITag[] Flatten()
    {
      List<ITag> tags;

      tags = new List<ITag>();

      this.FlattenTag(this, tags);

      return tags.ToArray();
    }

    public ITag[] GetAncestors()
    {
      List<ITag> tags;
      ITag tag;

      tags = new List<ITag>();
      tag = this.Parent;

      if (tag != null)
      {
        do
        {
          tags.Insert(0, tag);
          tag = tag.Parent;
        } while (tag != null);
      }

      return tags.ToArray();
    }

    public virtual void Remove()
    {
      if (!this.CanRemove)
        throw new TagException("Cannot remove this tag, parent not set or not supported.");

      ((ICollectionTag)this.Parent).Values.Remove(this);
    }

    public override string ToString()
    {
      return this.ToString(string.Empty);
    }

    public abstract string ToString(string indentString);

    public virtual string ToValueString()
    {
      return this.Value != null ? this.Value.ToString() : string.Empty;
    }

    protected virtual void OnNameChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.NameChanged;

      if (handler != null)
        handler(this, e);
    }

    protected virtual void OnParentChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.ParentChanged;

      if (handler != null)
        handler(this, e);
    }

    protected virtual void OnValueChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.ValueChanged;

      if (handler != null)
        handler(this, e);
    }

    private void FlattenTag(ITag tag, List<ITag> tags)
    {
      tags.Add(tag);
      if (tag is ICollectionTag)
      {
        foreach (ITag childTag in ((ICollectionTag)tag).Values)
          this.FlattenTag(childTag, tags);
      }
    }
  }
}