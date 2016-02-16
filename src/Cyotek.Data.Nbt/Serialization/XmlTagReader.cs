using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public class XmlTagReader : ITagReader
  {
    #region Fields

    private XmlReader _reader;

    #endregion

    #region Methods

    protected ITag ReadTag(ReadTagOptions options, TagType defaultTagType)
    {
      ITag result;
      TagType type;

      if (defaultTagType != TagType.None)
      {
        type = defaultTagType;
      }
      else
      {
        string typeName;

        typeName = _reader.GetAttribute("type");
        if (string.IsNullOrEmpty(typeName))
        {
          throw new InvalidDataException("Missing type attribute, unable to determine tag type.");
        }

        type = (TagType)Enum.Parse(typeof(TagType), typeName, true);
      }
      result = TagFactory.CreateTag(type);

      if ((options & ReadTagOptions.IgnoreName) == 0)
      {
        string name;

        name = _reader.GetAttribute("name");
        if (string.IsNullOrEmpty(name))
        {
          name = _reader.Name;
        }

        result.Name = name;
      }

      if ((options & ReadTagOptions.IgnoreValue) == 0)
      {
        switch (type)
        {
          case TagType.Byte:
            result.Value = this.ReadByte();
            break;

          case TagType.Short:
            result.Value = this.ReadShort();
            break;

          case TagType.Int:
            result.Value = this.ReadInt();
            break;

          case TagType.Long:
            result.Value = this.ReadLong();
            break;

          case TagType.Float:
            result.Value = this.ReadFloat();
            break;

          case TagType.Double:
            result.Value = this.ReadDouble();
            break;

          case TagType.ByteArray:
            result.Value = this.ReadByteArray();
            break;

          case TagType.String:
            result.Value = this.ReadString();
            break;

          case TagType.List:
            result.Value = this.ReadCollection((TagList)result);
            break;

          case TagType.Compound:
            result.Value = this.ReadDictionary((TagCompound)result);
            break;

          case TagType.IntArray:
            result.Value = this.ReadIntArray();
            break;

          default:
            throw new InvalidDataException($"Unrecognized tag type: {type}");
        }
      }

      return result;
    }

    private void LoadChildren(ICollection<ITag> value, ReadTagOptions options, TagType listType)
    {
      while (_reader.NodeType != XmlNodeType.EndElement && _reader.NodeType != XmlNodeType.None &&
             !_reader.IsEmptyElement)
      {
        _reader.Read();

        if (_reader.NodeType == XmlNodeType.Element)
        {
          value.Add(this.ReadTag(options, listType));
        }
      }

      if (_reader.NodeType == XmlNodeType.EndElement)
      {
        _reader.Read();
      }
    }

    #endregion

    #region ITagReader Interface

    public virtual bool IsNbtDocument(Stream stream)
    {
      bool result;
      long position;

      position = stream.Position;

      try
      {
        string typeName;

        _reader = XmlReader.Create(stream);

        while (!_reader.IsStartElement())
        {
          _reader.Read();
        }

        typeName = _reader.GetAttribute("type");

        result = typeName != null;
      }
      catch
      {
        result = false;
      }

      stream.Position = position;

      return result;
    }

    public virtual byte ReadByte()
    {
      return (byte)_reader.ReadElementContentAsInt();
    }

    public virtual byte[] ReadByteArray()
    {
      return this.ReadString().
                  Split(new[]
                        {
                          " ",
                          "\t",
                          "\n",
                          "\r"
                        }, StringSplitOptions.RemoveEmptyEntries).
                  Select(c => Convert.ToByte(c)).
                  ToArray();
    }

    public virtual TagCollection ReadCollection(TagList owner)
    {
      TagCollection value;
      TagType listType;
      string listTypeName;

      listTypeName = _reader.GetAttribute("limitType");
      if (string.IsNullOrEmpty(listTypeName))
      {
        throw new InvalidDataException("Missing limitType attribute, unable to determine list contents type.");
      }

      listType = (TagType)Enum.Parse(typeof(TagType), listTypeName, true);
      owner.ListType = listType;
      value = new TagCollection(owner, listType);

      this.LoadChildren(value, ReadTagOptions.IgnoreName, listType);

      return value;
    }

    public virtual TagDictionary ReadDictionary(TagCompound owner)
    {
      TagDictionary value;

      value = new TagDictionary(owner);

      this.LoadChildren(value, ReadTagOptions.None, TagType.None);

      return value;
    }

    public virtual TagCompound ReadDocument(Stream stream)
    {
      return this.ReadDocument(stream, ReadTagOptions.None);
    }

    public virtual TagCompound ReadDocument(Stream stream, ReadTagOptions options)
    {
      TagCompound result;

      _reader = XmlReader.Create(stream);

      while (!_reader.IsStartElement())
      {
        _reader.Read();
      }

      result = (TagCompound)this.ReadTag(options);

      return result;
    }

    public virtual double ReadDouble()
    {
      return _reader.ReadElementContentAsDouble();
    }

    public virtual float ReadFloat()
    {
      return _reader.ReadElementContentAsFloat();
    }

    public virtual int ReadInt()
    {
      return _reader.ReadElementContentAsInt();
    }

    public virtual int[] ReadIntArray()
    {
      return this.ReadString().
                  Split(new[]
                        {
                          " ",
                          "\t",
                          "\n",
                          "\r"
                        }, StringSplitOptions.RemoveEmptyEntries).
                  Select(c => Convert.ToInt32(c)).
                  ToArray();
    }

    public virtual long ReadLong()
    {
      return _reader.ReadElementContentAsLong();
    }

    public virtual short ReadShort()
    {
      return (short)_reader.ReadElementContentAsInt();
    }

    public virtual string ReadString()
    {
      string value;

      value = _reader.ReadElementContentAsString();
      if (string.IsNullOrEmpty(value))
      {
        value = null;
      }

      return value;
    }

    public virtual ITag ReadTag()
    {
      return this.ReadTag(ReadTagOptions.None);
    }

    public virtual ITag ReadTag(ReadTagOptions options)
    {
      return this.ReadTag(options, TagType.None);
    }

    #endregion
  }
}
