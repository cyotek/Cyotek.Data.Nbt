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

    [Category("Data")]
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
        }
      }
    }

    [Browsable(false)]
    public Tag Parent
    {
      get { return _parent; }
      set { _parent = value; }
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

    public abstract void SetValue(object value);

    public override string ToString()
    {
      return string.Concat("[", this.Type, ": ", this.Name, "=", this.ToValueString(), "]");
    }

    public abstract string ToValueString();

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
