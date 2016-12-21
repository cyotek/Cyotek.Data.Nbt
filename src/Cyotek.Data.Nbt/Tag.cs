using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public abstract class Tag : ITag
  {
    #region Fields

    private string _name;

    private ITag _parent;

    private object _value;

    #endregion

    #region Properties

    public virtual bool CanRemove
    {
      get { return this.Parent is ICollectionTag; }
    }

    public virtual string FullPath
    {
      get
      {
        StringBuilder results;
        ICollectionTag list;

        results = new StringBuilder();

        if (this.Parent != null)
        {
          results.Append(this.Parent.FullPath);
          results.Append(@"\");
        }

        list = this.Parent as ICollectionTag;
        if (list != null && list.IsList)
        {
          results.Append(list.Values.IndexOf(this));
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
          ICollectionTag collection;
          TagDictionary values;

          collection = this.Parent as ICollectionTag;
          values = collection?.Values as TagDictionary;

          values?.ChangeKey(this, value);

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

    #endregion

    #region Methods

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

    //public virtual byte[] GetValue()
    //{
    //  byte[] result;

    //  using (MemoryStream stream = new MemoryStream())
    //  {
    //    ITagWriter writer;

    //    writer = new BinaryTagWriter(stream);
    //    writer.WriteTag(this, WriteTagOptions.None);

    //    result = stream.ToArray();
    //  }

    //  return result;
    //}

    public virtual void Remove()
    {
      if (!this.CanRemove)
      {
        throw new TagException("Cannot remove this tag, parent not set or not supported.");
      }

      ((ICollectionTag)this.Parent).Values.Remove(this);
    }

    public abstract void SetValue(object value);

    public override string ToString()
    {
      return this.ToString(string.Empty);
    }

    public abstract string ToString(string indentString);

    public abstract string ToValueString();

    protected virtual void OnNameChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.NameChanged;

      handler?.Invoke(this, e);
    }

    protected virtual void OnParentChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.ParentChanged;

      handler?.Invoke(this, e);
    }

    protected virtual void OnValueChanged(EventArgs e)
    {
      EventHandler handler;

      handler = this.ValueChanged;

      handler?.Invoke(this, e);
    }

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

    #region ITag Interface

    [Category("Property Changed")]
    public event EventHandler NameChanged;

    [Category("Property Changed")]
    public event EventHandler ParentChanged;

    [Category("Property Changed")]
    public event EventHandler ValueChanged;

    public abstract object GetValue();

    ITag[] ITag.Flatten()
    {
      return this.Flatten();
    }

    ITag[] ITag.GetAncestors()
    {
      return this.GetAncestors();
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

    #endregion
  }
}
