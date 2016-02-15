using System;
using System.IO;
using System.Text;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public class XmlTagWriter : ITagWriter
  {
    #region Fields

    private XmlWriterSettings _settings;

    private XmlWriter _writer;

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

    #region ITagWriter2 Interface

    public virtual void WriteDocument(Stream stream, TagCompound tag)
    {
      this.WriteDocument(stream, tag, CompressionOption.Auto);
    }

    public virtual void WriteDocument(Stream stream, TagCompound tag, CompressionOption compression)
    {
      if (compression == CompressionOption.On)
      {
        throw new NotSupportedException("Compression is not supported.");
      }

      _settings = new XmlWriterSettings
                  {
                    Indent = true,
                    Encoding = Encoding.UTF8
                  };

      _writer = XmlWriter.Create(stream, _settings);
      _writer.WriteStartDocument(true);

      this.WriteTag(tag, WriteTagOptions.None);

      _writer.WriteEndDocument();
      _writer.Flush();
    }

    public virtual void WriteTag(ITag value)
    {
      this.WriteTag(value, WriteTagOptions.None);
    }

    public virtual void WriteTag(ITag value, WriteTagOptions options)
    {
      string name;

      name = value.Name;
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

      if (value.Type != TagType.End && (options & WriteTagOptions.IgnoreName) == 0)
      {
        this.WriteHeader(value);
      }

      switch (value.Type)
      {
        case TagType.End:
          // noop
          break;

        case TagType.Byte:
          this.WriteValue((byte)value.Value);
          break;

        case TagType.Short:
          this.WriteValue((short)value.Value);
          break;

        case TagType.Int:
          this.WriteValue((int)value.Value);
          break;

        case TagType.Long:
          this.WriteValue((long)value.Value);
          break;

        case TagType.Float:
          this.WriteValue((float)value.Value);
          break;

        case TagType.Double:
          this.WriteValue((double)value.Value);
          break;

        case TagType.ByteArray:
          this.WriteValue((byte[])value.Value);
          break;

        case TagType.String:
          this.WriteValue((string)value.Value);
          break;

        case TagType.List:
          this.WriteValue((TagCollection)value.Value);
          break;

        case TagType.Compound:
          this.WriteValue((TagDictionary)value.Value);
          break;

        case TagType.IntArray:
          this.WriteValue((int[])value.Value);
          break;

        default:
          throw new ArgumentException("Unrecognized or unsupported tag type.", nameof(value));
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
