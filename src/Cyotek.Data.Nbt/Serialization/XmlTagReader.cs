using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Cyotek.Data.Nbt.Serialization
{
  public class XmlTagReader : TagReader
  {
    #region Constants

    private static readonly char[] _arraySeparaters =
    {
      ' ',
      '\t',
      '\n',
      '\r'
    };

    #endregion

    #region Fields

    private readonly XmlReader _reader;

    #endregion

    #region Constructors

    public XmlTagReader(XmlReader reader)
    {
      _reader = reader;
    }

    public XmlTagReader(Stream stream)
    {
      _reader = XmlReader.Create(stream);
    }

    public XmlTagReader()
    { }

    #endregion

    #region Methods

    public override void Close()
    {
      base.Close();

      _reader.Close();
    }

    public override bool IsNbtDocument()
    {
      bool result;
      try
      {
        string typeName;

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

      return result;
    }

    public override byte ReadByte()
    {
      return (byte)_reader.ReadElementContentAsInt();
    }

    public override byte[] ReadByteArray()
    {
      return this.ReadString().Split(_arraySeparaters, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToByte(c)).ToArray();
    }

    public override TagDictionary ReadCompound()
    {
      TagDictionary value;

      value = new TagDictionary();

      _reader.Read();

      this.ReadChildValues(value, ReadTagOptions.None, TagType.None);

      return value;
    }

    public override TagCompound ReadDocument()
    {
      return this.ReadDocument(ReadTagOptions.None);
    }

    public override TagCompound ReadDocument(ReadTagOptions options)
    {
      TagCompound result;

      result = (TagCompound)this.ReadTag(options);

      return result;
    }

    public override double ReadDouble()
    {
      return _reader.ReadElementContentAsDouble();
    }

    public override float ReadFloat()
    {
      return _reader.ReadElementContentAsFloat();
    }

    public override int ReadInt()
    {
      return _reader.ReadElementContentAsInt();
    }

    public override int[] ReadIntArray()
    {
      return this.ReadString().Split(_arraySeparaters, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
    }

    public override TagCollection ReadList()
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
      value = new TagCollection(listType);

      _reader.Read();

      this.ReadChildValues(value, ReadTagOptions.IgnoreName, listType);

      return value;
    }

    public override long ReadLong()
    {
      return _reader.ReadElementContentAsLong();
    }

    public override short ReadShort()
    {
      return (short)_reader.ReadElementContentAsInt();
    }

    public override string ReadString()
    {
      string value;

      value = _reader.ReadElementContentAsString();
      if (string.IsNullOrEmpty(value))
      {
        value = null;
      }

      return value;
    }

    public override ITag ReadTag(ReadTagOptions options)
    {
      return this.ReadTag(options, TagType.None);
    }

    public override TagType ReadTagType()
    {
      TagType type;
      string typeName;

      typeName = _reader.GetAttribute("type");
      if (string.IsNullOrEmpty(typeName))
      {
        throw new InvalidDataException("Missing type attribute, unable to determine tag type.");
      }

      type = (TagType)Enum.Parse(typeof(TagType), typeName, true);

      return type;
    }

    protected ITag ReadTag(ReadTagOptions options, TagType defaultTagType)
    {
      ITag result;
      TagType type;
      string name;

      this.InitializeReader();

      type = this.ReadTagType(defaultTagType);

      if ((options & ReadTagOptions.IgnoreName) == 0)
      {
        name = _reader.GetAttribute("name");
        if (string.IsNullOrEmpty(name))
        {
          name = _reader.Name;
        }
      }
      else
      {
        name = string.Empty;
      }

      if ((options & ReadTagOptions.IgnoreValue) == 0)
      {
        switch (type)
        {
          case TagType.Byte:
            result = TagFactory.CreateTag(type, name, this.ReadByte());
            break;

          case TagType.Short:
            result = TagFactory.CreateTag(type, name, this.ReadShort());
            break;

          case TagType.Int:
            result = TagFactory.CreateTag(type, name, this.ReadInt());
            break;

          case TagType.Long:
            result = TagFactory.CreateTag(type, name, this.ReadLong());
            break;

          case TagType.Float:
            result = TagFactory.CreateTag(type, name, this.ReadFloat());
            break;

          case TagType.Double:
            result = TagFactory.CreateTag(type, name, this.ReadDouble());
            break;

          case TagType.ByteArray:
            result = TagFactory.CreateTag(type, name, this.ReadByteArray());
            break;

          case TagType.String:
            result = TagFactory.CreateTag(type, name, this.ReadString());
            break;

          case TagType.List:
            result = TagFactory.CreateTag(type, name, this.ReadList());
            break;

          case TagType.Compound:
            result = TagFactory.CreateTag(type, name, this.ReadCompound());
            break;

          case TagType.IntArray:
            result = TagFactory.CreateTag(type, name, this.ReadIntArray());
            break;

          default:
            throw new InvalidDataException($"Unrecognized tag type: {type}");
        }
      }
      else
      {
        result = TagFactory.CreateTag(type);
        result.Name = name;
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
  }
}
