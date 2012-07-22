using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagDouble : Tag
  {
    #region  Private Member Declarations

    const int PAYLOAD_SIZE = 8;

    #endregion  Private Member Declarations

    #region  Public Constructors

    public TagDouble()
      : this(string.Empty, 0)
    { }

    public TagDouble(string name)
      : this(name, 0)
    { }

    public TagDouble(double value)
      : this(string.Empty, value)
    { }

    public TagDouble(Stream input)
    {
      Name = TagString.ReadString(input);
      Value = ReadDouble(input);
    }

    public TagDouble(string name, double value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Double; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static double ReadDouble(Stream input)
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

      return BitConverter.ToDouble(bytes, 0);
    }

    internal static TagDouble ReadUnnamedTagDouble(Stream input)
    {
      return new TagDouble() { Value = ReadDouble(input) };
    }

    internal static void WriteDouble(Stream output, double value)
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
      return string.Format("{0}[Double: {1}={2}]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagDouble.WriteDouble(output, Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteDouble(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new double Value
    {
      get { return (double)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
