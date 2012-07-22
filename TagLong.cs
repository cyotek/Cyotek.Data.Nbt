using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagLong : Tag
  {
    #region  Private Member Declarations

    const int PAYLOAD_SIZE = 8;

    #endregion  Private Member Declarations

    #region  Public Constructors

    public TagLong()
      : this(string.Empty, 0)
    { }

    public TagLong(string name)
      : this(name, 0)
    { }

    public TagLong(long value)
      : this(string.Empty, value)
    { }

    public TagLong(Stream input)
    {
      Name = TagString.ReadString(input);
      Value = ReadLong(input);
    }

    public TagLong(string name, long value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Long; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static long ReadLong(Stream input)
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

      return BitConverter.ToInt64(bytes, 0);
    }

    internal static TagLong ReadUnnamedTagLong(Stream input)
    {
      return new TagLong() { Value = ReadLong(input) };
    }

    internal static void WriteLong(Stream output, long value)
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
      return string.Format("{0}[Long: {1}={2}]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagLong.WriteLong(output, Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteLong(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new long Value
    {
      get { return (long)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
