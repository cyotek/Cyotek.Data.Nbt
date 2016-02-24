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

    #region Constructors

    public XmlTagReader()
    { }

    public XmlTagReader(XmlReader reader)
      : this()
    {
      _reader = reader;
    }

    #endregion

    #region Methods

    protected ITag ReadTag(ReadTagOptions options, TagType defaultTagType)
    {
      ITag result;
      TagType type;

      this.InitializeReader();

      type = this.ReadTagType(defaultTagType);
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
        result.Value = this.ReadTagValue(result);
      }

      return result;
    }

    protected virtual object ReadTagValue(ITag tag)
    {
      object result;

      switch (tag.Type)
      {
        case TagType.Byte:
          result = this.ReadByte();
          break;

        case TagType.Short:
          result = this.ReadShort();
          break;

        case TagType.Int:
          result = this.ReadInt();
          break;

        case TagType.Long:
          result = this.ReadLong();
          break;

        case TagType.Float:
          result = this.ReadFloat();
          break;

        case TagType.Double:
          result = this.ReadDouble();
          break;

        case TagType.ByteArray:
          result = this.ReadByteArray();
          break;

        case TagType.String:
          result = this.ReadString();
          break;

        case TagType.List:
          result = this.ReadCollection((TagList)tag);
          break;

        case TagType.Compound:
          result = this.ReadDictionary((TagCompound)tag);
          break;

        case TagType.IntArray:
          result = this.ReadIntArray();
          break;

        default:
          throw new InvalidDataException($"Unrecognized tag type: {tag.Type}");
      }

      return result;
    }

    private void InitializeReader()
    {
      if (_reader.ReadState == ReadState.Initial)
      {
        while (!_reader.IsStartElement())
        {
          _reader.Read();
        }
      }
    }

    private void ReadChildValues(ICollection<ITag> value, ReadTagOptions options, TagType listType)
    {
      int depth;

      this.SkipWhitespace();

      depth = _reader.Depth;

      if (_reader.NodeType != XmlNodeType.EndElement)
      {
        do
        {
          if (_reader.NodeType == XmlNodeType.Element)
          {
            ITag child;

            child = this.ReadTag(options, listType);

            value.Add(child);
          }
          else
          {
            _reader.Read();
          }
        } while (_reader.Depth == depth);
      }
      else
      {
        _reader.Read();
        this.SkipWhitespace();
      }
    }

    private TagType ReadTagType(TagType defaultTagType)
    {
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

      return type;
    }

    private void SkipWhitespace()
    {
      while (_reader.NodeType == XmlNodeType.Whitespace)
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

      _reader.Read();

      this.ReadChildValues(value, ReadTagOptions.IgnoreName, listType);

      return value;
    }

    public virtual TagDictionary ReadDictionary(TagCompound owner)
    {
      TagDictionary value;

      value = new TagDictionary(owner);

      _reader.Read();

      this.ReadChildValues(value, ReadTagOptions.None, TagType.None);

      return value;
    }

    public virtual TagCompound ReadDocument(Stream stream)
    {
      return this.ReadDocument(stream, ReadTagOptions.None);
    }

    public virtual TagCompound ReadDocument(Stream stream, ReadTagOptions options)
    {
      TagCompound result;
      bool createReader;

      createReader = _reader == null;

      if (createReader)
      {
        _reader = XmlReader.Create(stream);
      }

      result = (TagCompound)this.ReadTag(options);

      if (createReader)
      {
        _reader = null;
      }

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
