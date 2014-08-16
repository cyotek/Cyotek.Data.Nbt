using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace Cyotek.Data.Nbt
{
  public class XmlTagReader : TagReader
  {
    #region Instance Fields

    private XmlReader _reader;

    #endregion

    #region Overridden Methods

    public override TagCompound Load(string fileName, NbtOptions options)
    {
      TagCompound result;

      if (string.IsNullOrEmpty(fileName))
      {
        throw new ArgumentNullException("fileName");
      }

      this.Options = options;

      using (Stream fileStream = File.OpenRead(fileName))
      {
        this.InputStream = fileStream;
        result = (TagCompound)this.Read(options);
      }

      return result;
    }

    public override ITag Read(NbtOptions options)
    {
      return this.Read(options, TagType.None);
    }

    public override byte ReadByte()
    {
      return (byte)_reader.ReadElementContentAsInt();
    }

    public override byte[] ReadByteArray()
    {
      return this.ReadString().Split(new[]
                                     {
                                       " ", "\t", "\n", "\r"
                                     }, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToByte(c)).ToArray();
    }

    public override TagCollection ReadCollection(TagList owner)
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

      this.LoadChildren(value, NbtOptions.None, listType);

      return value;
    }

    public override TagDictionary ReadDictionary(TagCompound owner)
    {
      TagDictionary value;

      value = new TagDictionary(owner);

      this.LoadChildren(value, this.Options, TagType.None);

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
      return this.ReadString().Split(new[]
                                     {
                                       " ", "\t", "\n", "\r"
                                     }, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
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

    protected override void OnInputStreamChanged(EventArgs e)
    {
      base.OnInputStreamChanged(e);

      _reader = XmlReader.Create(this.InputStream);

      while (!_reader.IsStartElement())
      {
        _reader.Read();
      }
    }

    #endregion

    #region Protected Members

    protected ITag Read(NbtOptions options, TagType defaultTagType)
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

      if ((options & NbtOptions.ReadHeader) != 0)
      {
        string name;

        name = _reader.GetAttribute("name");
        if (string.IsNullOrEmpty(name))
        {
          name = _reader.Name;
        }

        result.Name = name;
      }

      if ((options & NbtOptions.HeaderOnly) == 0)
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
            throw new InvalidDataException(string.Format("Unrecognized tag type: {0}", type));
        }
      }

      return result;
    }

    #endregion

    #region Private Members

    private void LoadChildren(ICollection<ITag> value, NbtOptions options, TagType listType)
    {
      while (_reader.NodeType != XmlNodeType.EndElement && _reader.NodeType != XmlNodeType.None && !_reader.IsEmptyElement)
      {
        _reader.Read();

        if (_reader.NodeType == XmlNodeType.Element)
        {
          value.Add(this.Read(options, listType));
        }
      }

      if (_reader.NodeType == XmlNodeType.EndElement)
      {
        _reader.Read();
      }
    }

    #endregion
  }
}
