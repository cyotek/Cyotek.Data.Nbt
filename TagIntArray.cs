using System;
using System.IO;
using System.Text;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagIntArrayEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagIntArray : Tag
  {
    #region  Public Constructors

    public TagIntArray()
      : this(string.Empty, new int[0])
    { }

    public TagIntArray(string name)
      : this(name, new int[0])
    { }

    public TagIntArray(int[] value)
      : this(string.Empty, value)
    { }

    public TagIntArray(Stream input)
    {
      this.Name = TagString.ReadString(input);
      this.Value = ReadIntArray(input);
    }

    public TagIntArray(string name, int[] value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Private Class Methods

    private static void WriteIntArray(Stream output, int[] value)
    {
      if (value != null)
      {
        TagInt.WriteInt(output, value.Length);
        for (int i = 0; i < value.Length; i++)
        {
          TagInt.WriteInt(output, value[i]);
        }
      }
      else
      {
        TagInt.WriteInt(output, 0);
      }
    }

    #endregion  Private Class Methods

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.IntArray; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static int[] ReadIntArray(Stream input)
    {
      int length = TagInt.ReadInt(input);
      int bufferLength = length * 4;

      byte[] buffer = new byte[bufferLength];
      if (bufferLength != input.Read(buffer, 0, bufferLength))
      {
        throw new Exception();
      }

      int[] ints = new int[length];
      for (int i = 0; i < length; i++)
      {
        if (BitConverter.IsLittleEndian)
        {
          BitHelper.SwapBytes(buffer, i * 4, 4);
        }

        ints[i] = BitConverter.ToInt32(buffer, i * 4);
      }

      buffer = null;

      return ints;
    }

    internal static ITag ReadUnnamedTagIntArray(Stream input)
    {
      return new TagIntArray() { Value = ReadIntArray(input) };
    }

    #endregion  Internal Class Methods

    #region  Public Overridden Methods

    public override string ToString(string indentString)
    {
      StringBuilder sb = new StringBuilder();

      if (Value == null || Value.Length == 0)
      {
        return string.Format("{0}[Int_Array: {1}]", indentString, Name);
      }

      sb.AppendLine(string.Format("{0}[Int_Array: {1}", indentString, Name));

      foreach (int item in Value)
      {
        sb.AppendLine(string.Format("{0}  {1}", indentString, item));
      }

      sb.AppendLine(string.Format("{0}]", indentString));

      return sb.ToString();
    }

    public override void Write(System.IO.Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagIntArray.WriteIntArray(output, Value);
    }

    public override void WriteUnnamed(System.IO.Stream output)
    {
      WriteIntArray(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new int[] Value
    {
      get { return (int[])base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
