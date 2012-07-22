using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagShort : Tag
  {
    #region  Private Member Declarations

    const int PAYLOAD_SIZE = 2;

    #endregion  Private Member Declarations

    #region  Public Constructors

    public TagShort()
      : this(string.Empty, 0)
    { }

    public TagShort(string name)
      : this(name, 0)
    { }

    public TagShort(short value)
      : this(string.Empty, value)
    { }

    public TagShort(Stream input)
    {
      Name = TagString.ReadString(input);
      Value = ReadShort(input);
    }

    public TagShort(string name, short value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Short; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static short ReadShort(Stream input)
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

      return BitConverter.ToInt16(bytes, 0);
    }

    internal static TagShort ReadUnnamedTagShort(Stream input)
    {
      return new TagShort() { Value = ReadShort(input) };
    }

    internal static void WriteShort(Stream output, short value)
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
      return string.Format("{0}[Short: {1}={2}]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagShort.WriteShort(output, Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteShort(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new short Value
    {
      get { return (short)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
