using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagByte : Tag
  {
    #region  Public Constructors

    public TagByte()
      : this(string.Empty, 0)
    { }

    public TagByte(string name)
      : this(name, 0)
    { }

    public TagByte(byte value)
      : this(string.Empty, value)
    { }

    public TagByte(Stream decompressedInput)
    {
      Name = TagString.ReadString(decompressedInput);
      Value = ReadByte(decompressedInput);
    }

    public TagByte(string name, byte value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Byte; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static byte ReadByte(Stream decompressedInput)
    {
      int temp = decompressedInput.ReadByte();
      if (temp == (temp & 0xFF))
      {
        return (byte)temp;
      }
      else
      {
        throw new Exception();
      }
    }

    internal static TagByte ReadUnnamedTagByte(Stream input)
    {
      return new TagByte() { Value = ReadByte(input) };
    }

    internal static void WriteByte(Stream output, byte value)
    {
      output.WriteByte(value);
    }

    #endregion  Internal Class Methods

    #region  Public Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Byte: {1}={2}]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagByte.WriteByte(output, Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteByte(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new byte Value
    {
      get { return (byte)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
