using System;
using System.Collections.Generic;
using System.IO;
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

    private readonly XmlReader _reader;

    private readonly TagState _state;

    #endregion

    #region Constructors

    public XmlTagReader(XmlReader reader)
    {
      _reader = reader;

      _state = new TagState(FileAccess.Read);
      _state.Start();
    }

    public XmlTagReader(Stream stream)
      : this(XmlReader.Create(stream))
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
      byte[] result;
      string value;

      value = this.ReadString();

      if (!string.IsNullOrEmpty(value))
      {
        string[] values;

        values = value.Split(_arraySeparaters, StringSplitOptions.RemoveEmptyEntries);
        result = new byte[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
          result[i] = Convert.ToByte(values[i]);
        }
      }
      else
      {
        result = TagByteArray.EmptyValue;
      }

      return result;
    }

    public override TagDictionary ReadCompound()
    {
      TagDictionary value;

      value = new TagDictionary();

      _reader.Read();

      this.ReadChildValues(value, TagType.None);

      return value;
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
      int[] result;
      string value;

      value = this.ReadString();

      if (!string.IsNullOrEmpty(value))
      {
        string[] values;

        values = value.Split(_arraySeparaters, StringSplitOptions.RemoveEmptyEntries);
        result = new int[values.Length];

        for (int i = 0; i < values.Length; i++)
        {
          result[i] = Convert.ToInt32(values[i]);
        }
      }
      else
      {
        result = TagIntArray.EmptyValue;
      }

      return result;
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

      this.ReadChildValues(value, listType);

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

    public override Tag ReadTag()
    {
      return this.ReadTag(TagType.None);
    }

    public override string ReadTagName()
    {
      return _reader.Name;
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

    private void ReadChildValues(ICollection<Tag> value, TagType listType)
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
            Tag child;

            child = this.ReadTag(listType);

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

    private Tag ReadTag(TagType defaultTagType)
    {
      Tag result;
      TagType type;
      string name;
      TagContainerState state;

      this.InitializeReader();

      type = this.ReadTagType(defaultTagType);

      state = _state.StartTag(type);

      if (type != TagType.End && (state == null || state.ContainerType != TagType.List))
      {
        name = _reader.GetAttribute("name");
        if (string.IsNullOrEmpty(name))
        {
          name = this.ReadTagName();
        }
      }
      else
      {
        name = string.Empty;
      }

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

      _state.EndTag();

      return result;
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
