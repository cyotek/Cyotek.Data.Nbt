using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagInt : Tag
  {
    #region  Private Member Declarations

    const int PAYLOAD_SIZE = 4;

    #endregion  Private Member Declarations

    #region  Public Constructors

    public TagInt()
      : this(string.Empty, 0)
    { }

    public TagInt(string name)
      : this(name, 0)
    { }

    public TagInt(int value)
      : this(string.Empty, value)
    { }

    public TagInt(Stream input)
    {
      Name = TagString.ReadString(input);
      Value = ReadInt(input);
    }

    public TagInt(string name, int value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Int; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static int ReadInt(Stream input)
    {

      byte[] bytes = new byte[PAYLOAD_SIZE];
      if (PAYLOAD_SIZE != input.Read(bytes, 0, PAYLOAD_SIZE))
      {
        throw new Exception();
      }

      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
      }

      return BitConverter.ToInt32(bytes, 0);
    }

    internal static TagInt ReadUnnamedTagInt(Stream input)
    {
      return new TagInt() { Value = ReadInt(input) };
    }

    internal static void WriteInt(Stream output, int value)
    {
      byte[] bytes = BitConverter.GetBytes(value);
      if (BitConverter.IsLittleEndian)
      {
        BitHelper.SwapBytes(bytes, 0, PAYLOAD_SIZE);
      }

      output.Write(bytes, 0, PAYLOAD_SIZE);
    }

    #endregion  Internal Class Methods

    #region  Public Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Int: {1}={2}]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagInt.WriteInt(output, Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteInt(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new int Value
    {
      get { return (int)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
