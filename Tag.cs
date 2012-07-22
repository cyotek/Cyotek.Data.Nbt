using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Cyotek.Data.Nbt
{
  public abstract class Tag : ITag
  {
    #region  Private Member Declarations

    private string _name;
    private ITag _parent;
    private object _value;

    #endregion  Private Member Declarations

    #region  Events

    [Category("Property Changed")]
    public event EventHandler NameChanged;

    [Category("Property Changed")]
    public event EventHandler ParentChanged;

    [Category("Property Changed")]
    public event EventHandler ValueChanged;

    #endregion  Events

    #region  Public Class Methods

    public static ITag Read(Stream input)
    {
      int temp = input.ReadByte();
      if (temp != (temp & 0xFF))
      {
        throw new Exception();
      }

      switch ((TagType)temp)
      {
        case TagType.End:
          return new TagEnd();
        case TagType.Byte:
          return new TagByte(input);
        case TagType.Short:
          return new TagShort(input);
        case TagType.Int:
          return new TagInt(input);
        case TagType.IntArray:
          return new TagIntArray(input);
        case TagType.Long:
          return new TagLong(input);
        case TagType.Float:
          return new TagFloat(input);
        case TagType.Double:
          return new TagDouble(input);
        case TagType.ByteArray:
          return new TagByteArray(input);
        case TagType.String:
          return new TagString(input);
        case TagType.List:
          return new TagList(input);
        case TagType.Compound:
          return new TagCompound(input);
        default:
          throw new NotImplementedException();
      }
    }

    public static ITag ReadFromFile(string filename)
    {
      ITag tag = null;
      //Check if gzipped stream
      try
      {
        using (FileStream input = File.OpenRead(filename))
        {
          using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress))
          {
            tag = Tag.Read(gzipStream);
          }
        }
      }
      catch (Exception)
      {
        tag = null;
      }

      if (tag != null)
      {
        return tag;
      }

      //Try Deflate stream
      try
      {
        using (FileStream input = File.OpenRead(filename))
        {
          using (DeflateStream deflateStream = new DeflateStream(input, CompressionMode.Decompress))
          {
            tag = Tag.Read(deflateStream);
          }
        }
      }
      catch (Exception)
      {
        tag = null;
      }

      if (tag != null)
      {
        return tag;
      }

      //Assume uncompressed stream
      using (FileStream input = File.OpenRead(filename))
      {
        tag = Tag.Read(input);
      }

      return tag;
    }

    public static ITag ReadFromGzippedFile(string filename)
    {
      using (FileStream input = File.OpenRead(filename))
      {
        using (GZipStream gzipStream = new GZipStream(input, CompressionMode.Decompress))
        {
          return Tag.Read(gzipStream);
        }
      }
    }

    #endregion  Public Class Methods

    #region  Public Overridden Methods

    public override string ToString()
    {
      return ToString(string.Empty);
    }

    #endregion  Public Overridden Methods

    #region  Public Abstract Methods

    public abstract string ToString(string indentString);

    public abstract void Write(Stream output);

    public abstract void WriteUnnamed(Stream output);

    #endregion  Public Abstract Methods

    #region  Public Methods

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
        throw new InvalidOperationException("Cannot remove this tag, parent not set or not supported.");

      ((ICollectionTag)this.Parent).Values.Remove(this);
    }

    public virtual string ToValueString()
    {
      return this.Value != null ? this.Value.ToString() : string.Empty;
    }

    #endregion  Public Methods

    #region  Public Properties

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

    [Category(""), DefaultValue(null), Browsable(false)]
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

    #endregion  Public Properties

    #region  Private Methods

    private void FlattenTag(ITag tag, List<ITag> tags)
    {
      tags.Add(tag);
      if (tag is ICollectionTag)
      {
        foreach (ITag childTag in ((ICollectionTag)tag).Values)
          this.FlattenTag(childTag, tags);
      }
    }

    #endregion  Private Methods

    #region  Protected Methods

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

    #endregion  Protected Methods
  }
}
