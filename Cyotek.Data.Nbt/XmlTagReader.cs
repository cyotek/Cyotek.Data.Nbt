using System;
using System.IO;
using System.Linq;
using System.Xml;

namespace Cyotek.Data.Nbt
{
  public class XmlTagReader
    : TagReader
  {
    private XmlReader _reader;

    public override TagCompound Load(string fileName, NbtOptions options)
    {
      TagCompound result;

      if (string.IsNullOrEmpty(fileName))
        throw new ArgumentNullException("fileName");

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
      return this.ReadString().Split(new string[] { " ", "\t", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToByte(c)).ToArray();
    }

    public override TagCollection ReadCollection(TagList owner)
    {
      TagCollection value;
      TagType listType;

      listType = (TagType)Enum.Parse(typeof(TagType), _reader.GetAttribute("limitType"), true);
      owner.ListType = listType;
      value = new TagCollection(owner, listType);

      while (_reader.NodeType != XmlNodeType.EndElement)
      {
        _reader.Read();

        if (_reader.NodeType == XmlNodeType.Element)
          value.Add(this.Read(NbtOptions.None, listType));
      }

      _reader.Read();

      return value;
    }

    public override TagDictionary ReadDictionary(TagCompound owner)
    {
      TagDictionary value;

      value = new TagDictionary(owner);

      while (_reader.NodeType != XmlNodeType.EndElement)
      {
        _reader.Read();

        if (_reader.NodeType == XmlNodeType.Element)
          value.Add(this.Read());
      }

      _reader.Read();

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
      return this.ReadString().Split(new string[] { " ", "\t", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries).Select(c => Convert.ToInt32(c)).ToArray();
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
      return _reader.ReadElementContentAsString();
    }

    protected override void OnInputStreamChanged(EventArgs e)
    {
      base.OnInputStreamChanged(e);

      _reader = XmlReader.Create(this.InputStream);

      this.IncrementReader(_reader);
    }

    protected ITag Read(NbtOptions options, TagType defaultTagType)
    {
      ITag result;
      TagType type;
      string name;

      name = _reader.GetAttribute("name");
      if (string.IsNullOrEmpty(name))
        name = _reader.Name;

      type = defaultTagType != TagType.None ? defaultTagType : (TagType)Enum.Parse(typeof(TagType), _reader.GetAttribute("type"), true);
      result = TagFactory.CreateTag(type);

      if (options.HasFlag(NbtOptions.Header))
        result.Name = name;

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
          throw new NotImplementedException(string.Format("Unrecognized tag type: {0}", type));
      }

      return result;
    }

    private void IncrementReader(XmlReader reader)
    {
      while (!reader.IsStartElement())
        reader.Read();
    }
  }
}