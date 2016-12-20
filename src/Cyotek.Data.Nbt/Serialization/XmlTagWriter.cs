using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public class XmlTagWriter : ITagWriter
  {
    #region Fields

    private XmlWriter _writer;

    #endregion

    #region Constructors

    public XmlTagWriter()
    { }

    public XmlTagWriter(XmlWriter writer)
      : this()
    {
      _writer = writer;
    }

    #endregion

    #region Methods

    protected void WriteHeader(ITag value)
    {
      _writer.WriteAttributeString("type", value.Type.ToString());
    }

    protected virtual void WriteValue(TagCollection value)
    {
      _writer.WriteAttributeString("limitType", value.LimitType.ToString());

      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.IgnoreName);
      }
    }

    protected virtual void WriteValue(TagDictionary value)
    {
      foreach (ITag item in value)
      {
        this.WriteTag(item, WriteTagOptions.None);
      }
    }

    #endregion

    #region ITagWriter Interface

    public virtual void WriteDocument(Stream stream, TagCompound tag)
    {
      this.WriteDocument(stream, tag, CompressionOption.Auto);
    }

    public virtual void WriteDocument(Stream stream, TagCompound tag, CompressionOption compression)
    {
      bool createWriter;

      if (compression == CompressionOption.On)
      {
        throw new NotSupportedException("Compression is not supported.");
      }

      createWriter = _writer == null;

      if (createWriter)
      {
        XmlWriterSettings settings;

        settings = new XmlWriterSettings
                   {
                     Indent = true,
                     Encoding = Encoding.UTF8
                   };

        _writer = XmlWriter.Create(stream, settings);
        _writer.WriteStartDocument(true);
      }

      this.WriteTag(tag, WriteTagOptions.None);

      if (createWriter)
      {
        _writer.WriteEndDocument();
        _writer.Flush();
        _writer = null;
      }
    }

    public virtual void WriteTag(ITag tag)
    {
      this.WriteTag(tag, WriteTagOptions.None);
    }

    public virtual void WriteTag(ITag tag, WriteTagOptions options)
    {
      string name;

      name = tag.Name;
      if (string.IsNullOrEmpty(name))
      {
        name = "tag";
      }

      if (XmlConvert.EncodeName(name) == name)
      {
        _writer.WriteStartElement(name);
      }
      else
      {
        _writer.WriteStartElement("tag");
        _writer.WriteAttributeString("name", name);
      }

      if (tag.Type != TagType.End && (options & WriteTagOptions.IgnoreName) == 0)
      {
        this.WriteHeader(tag);
      }

      switch (tag.Type)
      {
        case TagType.End:
          // noop
          break;

        case TagType.Byte:
          this.WriteValue((byte)tag.GetValue());
          break;

        case TagType.Short:
          this.WriteValue((short)tag.GetValue());
          break;

        case TagType.Int:
          this.WriteValue((int)tag.GetValue());
          break;

        case TagType.Long:
          this.WriteValue((long)tag.GetValue());
          break;

        case TagType.Float:
          this.WriteValue((float)tag.GetValue());
          break;

        case TagType.Double:
          this.WriteValue((double)tag.GetValue());
          break;

        case TagType.ByteArray:
          this.WriteValue((byte[])tag.GetValue());
          break;

        case TagType.String:
          this.WriteValue((string)tag.GetValue());
          break;

        case TagType.List:
          this.WriteValue((TagCollection)tag.GetValue());
          break;

        case TagType.Compound:
          this.WriteValue((TagDictionary)tag.GetValue());
          break;

        case TagType.IntArray:
          this.WriteValue((int[])tag.GetValue());
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", nameof(tag));
      }

      _writer.WriteEndElement();
    }

    public virtual void WriteValue(string value)
    {
      if (value != null)
      {
        if (value.Contains("<") || value.Contains(">") || value.Contains("&"))
        {
          _writer.WriteCData(value);
        }
        else
        {
          _writer.WriteValue(value);
        }
      }
    }

    public virtual void WriteValue(short value)
    {
      _writer.WriteValue(value);
    }

    public virtual void WriteValue(long value)
    {
      _writer.WriteValue(value);
    }

    public virtual void WriteValue(int[] value)
    {
      StringBuilder output;

      output = new StringBuilder();

      foreach (int i in value)
      {
        if (output.Length != 0)
        {
          output.Append(" ");
        }

        output.Append(i);
      }

      this.WriteValue(output.ToString());
    }

    public virtual void WriteValue(int value)
    {
      _writer.WriteValue(value);
    }

    public virtual void WriteValue(float value)
    {
      _writer.WriteValue(value);
    }

    public virtual void WriteValue(double value)
    {
      _writer.WriteValue(value);
    }

    public virtual void WriteValue(byte value)
    {
      _writer.WriteValue(value);
    }

    public virtual void WriteValue(byte[] value)
    {
      StringBuilder output;

      output = new StringBuilder();

      foreach (byte i in value)
      {
        if (output.Length != 0)
        {
          output.Append(" ");
        }

        output.Append(i);
      }

      this.WriteValue(output.ToString());
    }

    #endregion
  }
}
