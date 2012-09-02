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
    { get { return this.Parent != null && this.Parent is ITagCollection; } }

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

        if (this.Parent is ITagCollection && ((ITagCollection)this.Parent).IsList)
          results.Append(((ITagCollection)this.Parent).Values.IndexOf(this));
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
          if (this.Parent != null && this.Parent is ITagCollection && ((ITagCollection)this.Parent).Values is TagDictionary)
            ((TagDictionary)((ITagCollection)this.Parent).Values).ChangeKey(this, value);

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

    public static ITag Read(Stream input)
    {
      int rawType;
      ITag result;

      rawType = input.ReadByte();

      switch ((TagType)rawType)
      {
        case TagType.End:
          result = new TagEnd();
          break;

        case TagType.Byte:
          result = new TagByte(input);
          break;

        case TagType.Short:
          result = new TagShort(input);
          break;

        case TagType.Int:
          result = new TagInt(input);
          break;

        case TagType.IntArray:
          result = new TagIntArray(input);
          break;

        case TagType.Long:
          result = new TagLong(input);
          break;

        case TagType.Float:
          result = new TagFloat(input);
          break;

        case TagType.Double:
          result = new TagDouble(input);
          break;

        case TagType.ByteArray:
          result = new TagByteArray(input);
          break;

        case TagType.String:
          result = new TagString(input);
          break;

        case TagType.List:
          result = new TagList(input);
          break;

        case TagType.Compound:
          result = new TagCompound(input);
          break;
        default:
          throw new NotImplementedException(string.Format("Unrecognized tag type: {0}", rawType));
      }

      return result;
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

      ((ITagCollection)this.Parent).Values.Remove(this);
    }

    public override string ToString()
    {
      return ToString(string.Empty);
    }

    public abstract string ToString(string indentString);

    public virtual string ToValueString()
    {
      return this.Value != null ? this.Value.ToString() : string.Empty;
    }

    public abstract void Write(Stream output);

    public abstract void WriteUnnamed(Stream output);

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
      if (tag is ITagCollection)
      {
        foreach (ITag childTag in ((ITagCollection)tag).Values)
          this.FlattenTag(childTag, tags);
      }
    }
  }
}