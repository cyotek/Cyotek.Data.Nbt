using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public abstract class Tag : ITag
  {
    #region Instance Fields

    private string _name;

    private ITag _parent;

    private object _value;

    #endregion

    #region Events

    [Category("Property Changed")]
    public event EventHandler NameChanged;

    [Category("Property Changed")]
    public event EventHandler ParentChanged;

    [Category("Property Changed")]
    public event EventHandler ValueChanged;

    #endregion

    #region Overridden Methods

    public override string ToString()
    {
      return this.ToString(string.Empty);
    }

    #endregion

    #region Public Properties

    public virtual bool CanRemove
    {
      get { return this.Parent is ICollectionTag; }
    }

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
        {
          results.Append(((ICollectionTag)this.Parent).Values.IndexOf(this));
        }
        else
        {
          results.Append(this.Name);
        }

        return results.ToString();
      }
    }

    [Category("")]
    [DefaultValue("")]
    public virtual string Name
    {
      get { return _name; }
      set
      {
        if (this.Name != value)
        {
          if (this.Parent is ICollectionTag && ((ICollectionTag)this.Parent).Values is TagDictionary)
          {
            ((TagDictionary)((ICollectionTag)this.Parent).Values).ChangeKey(this, value);
          }

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

    [Category("")]
    [DefaultValue(null)]
    public virtual object Value
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

    #endregion

    #region Public Members

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

      while (tag != null)
      {
        tags.Insert(0, tag);
        tag = tag.Parent;
      }

      return tags.ToArray();
    }

    public virtual byte[] GetValue()
    {
      byte[] result;

      using (MemoryStream stream = new MemoryStream())
      {
        TagWriter writer;

        writer = new BinaryTagWriter(stream, NbtOptions.ReadHeader);
        writer.Write(this);

        result = stream.ToArray();
      }

      return result;
    }

    public virtual void Remove()
    {
      if (!this.CanRemove)
      {
        throw new TagException("Cannot remove this tag, parent not set or not supported.");
      }

      ((ICollectionTag)this.Parent).Values.Remove(this);
    }

    public abstract string ToString(string indentString);

    public virtual string ToValueString()
    {
      return this.Value != null ? this.Value.ToString() : string.Empty;
    }

    #endregion

    #region Protected Members

    protected virtual void OnNameChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.NameChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnParentChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.ParentChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    protected virtual void OnValueChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.ValueChanged;

      if (handler != null)
      {
        handler(this, e);
      }
    }

    #endregion

    #region Private Members

    private void FlattenTag(ITag tag, List<ITag> tags)
    {
      ICollectionTag collectionTag;

      tags.Add(tag);

      collectionTag = tag as ICollectionTag;
      if (collectionTag != null)
      {
        foreach (ITag childTag in collectionTag.Values)
        {
          this.FlattenTag(childTag, tags);
        }
      }
    }

    #endregion

    #region ITag Members

    bool ITag.CanRemove
    {
      get { return this.CanRemove; }
    }

    string ITag.FullPath
    {
      get { return this.FullPath; }
    }

    string ITag.Name
    {
      get { return this.Name; }
      set { this.Name = value; }
    }

    ITag ITag.Parent
    {
      get { return this.Parent; }
      set { this.Parent = value; }
    }

    TagType ITag.Type
    {
      get { return this.Type; }
    }

    object ITag.Value
    {
      get { return this.Value; }
      set { this.Value = value; }
    }

    ITag[] ITag.Flatten()
    {
      return this.Flatten();
    }

    ITag[] ITag.GetAncestors()
    {
      return this.GetAncestors();
    }

    byte[] ITag.GetValue()
    {
      return this.GetValue();
    }

    void ITag.Remove()
    {
      this.Remove();
    }

    string ITag.ToString()
    {
      return this.ToString();
    }

    string ITag.ToString(string indent)
    {
      return this.ToString(indent);
    }

    string ITag.ToValueString()
    {
      return this.ToValueString();
    }

    #endregion
  }
}
