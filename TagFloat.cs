using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagFloat : Tag
  {
    #region  Private Member Declarations

    const int PAYLOAD_SIZE = 4;

    #endregion  Private Member Declarations

    #region  Public Constructors

    public TagFloat()
      : this(string.Empty, 0)
    { }

    public TagFloat(string name)
      : this(name, 0)
    { }

    public TagFloat(float value)
      : this(string.Empty, value)
    { }

    public TagFloat(Stream input)
    {
      Name = TagString.ReadString(input);
      Value = ReadFloat(input);
    }

    public TagFloat(string name, float value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.Float; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static float ReadFloat(Stream input)
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

      return BitConverter.ToSingle(bytes, 0);
    }

    internal static TagFloat ReadUnnamedTagFloat(Stream input)
    {
      return new TagFloat() { Value = ReadFloat(input) };
    }

    internal static void WriteFloat(Stream output, float value)
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
      return string.Format("{0}[Float: {1}={2}]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagFloat.WriteFloat(output, Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteFloat(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new float Value
    {
      get { return (float)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
