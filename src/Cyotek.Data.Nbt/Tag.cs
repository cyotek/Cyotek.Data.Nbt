using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public abstract class Tag
  {
    #region Fields

    private string _name;

    private Tag _parent;

    #endregion

    #region Constructors

    protected Tag()
    {
      _name = string.Empty;
    }

    protected Tag(string name)
    {
      _name = name;
    }

    #endregion

    #region Events

    [Category("Property Changed")]
    public event EventHandler NameChanged;

    [Category("Property Changed")]
    public event EventHandler ParentChanged;

    [Category("Property Changed")]
    public event EventHandler ValueChanged;

    #endregion

    #region Properties

    public virtual string FullPath
    {
      get
      {
        StringBuilder results;
        ICollectionTag list;

        results = new StringBuilder();

        if (_parent != null)
        {
          results.Append(_parent.FullPath);
          results.Append(@"\");
        }

        list = _parent as ICollectionTag;
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
    public string Name
    {
      get { return _name; }
      set
      {
        if (this.Name != value)
        {
          ICollectionTag collection;
          TagDictionary values;

          collection = _parent as ICollectionTag;
          values = collection?.Values as TagDictionary;

          values?.ChangeKey(this, value);

          _name = value;

          this.OnNameChanged(EventArgs.Empty);
        }
      }
    }

    [Browsable(false)]
    public Tag Parent
    {
      get { return _parent; }
      set
      {
        if (_parent != value)
        {
          _parent = value;

          this.OnParentChanged(EventArgs.Empty);
        }
      }
    }

    public abstract TagType Type { get; }

    #endregion

    #region Methods

    public Tag[] Flatten()
    {
      List<Tag> tags;

      tags = new List<Tag>();

      this.FlattenTag(this, tags);

      return tags.ToArray();
    }

    public Tag[] GetAncestors()
    {
      List<Tag> tags;
      Tag tag;

      tags = new List<Tag>();
      tag = _parent;

      while (tag != null)
      {
        tags.Insert(0, tag);
        tag = tag.Parent;
      }

      return tags.ToArray();
    }

    public abstract object GetValue();

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
      ICollectionTag parent;

      parent = _parent as ICollectionTag;

      if (parent == null)
      {
        throw new TagException("Cannot remove this tag, parent not set or not supported.");
      }

      parent.Values.Remove(this);
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

    private void FlattenTag(Tag tag, List<Tag> tags)
    {
      ICollectionTag collectionTag;

      tags.Add(tag);

      collectionTag = tag as ICollectionTag;
      if (collectionTag != null)
      {
        foreach (Tag childTag in collectionTag.Values)
        {
          this.FlattenTag(childTag, tags);
        }
      }
    }

    #endregion
  }
}
