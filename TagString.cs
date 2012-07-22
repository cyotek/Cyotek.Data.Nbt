using System;
using System.IO;
using System.Text;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagTextEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagString : Tag
  {
    #region  Public Constructors

    public TagString()
      : this(string.Empty, string.Empty)
    { }

    public TagString(string name)
      : this(name, string.Empty)
    { }

    public TagString(Stream input)
    {
      this.Name = TagString.ReadString(input);
      this.Value = TagString.ReadString(input);
    }

    public TagString(string name, string value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion  Public Constructors

    #region  Public Class Methods

    public static string ReadString(Stream input)
    {
      short length = TagShort.ReadShort(input);
      byte[] bytes = new byte[length];
      if (length != input.Read(bytes, 0, length))
      {
        throw new Exception();
      }

      return Encoding.UTF8.GetString(bytes);
    }

    #endregion  Public Class Methods

    #region  Overriden Properties

    public override TagType Type
    {
      get { return TagType.String; }
    }

    #endregion  Overriden Properties

    #region  Internal Class Methods

    internal static TagString ReadUnnamedTagString(Stream input)
    {
      return new TagString() { Value = ReadString(input) };
    }

    internal static void WriteString(Stream output, string value)
    {
      byte[] bytes;

      if (value == null)
        bytes = new byte[0];
      else
        bytes = Encoding.UTF8.GetBytes(value);

      TagShort.WriteShort(output, (short)bytes.Length);

      output.Write(bytes, 0, bytes.Length);
    }

    #endregion  Internal Class Methods

    #region  Public Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[string: {1}=\"{2}\"]", indentString, Name, Value);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
      TagString.WriteString(output, Name);
      TagString.WriteString(output, this.Value);
    }

    public override void WriteUnnamed(Stream output)
    {
      WriteString(output, Value);
    }

    #endregion  Public Overridden Methods

    #region  Public Properties

    public new string Value
    {
      get { return (string)base.Value; }
      set { base.Value = value; }
    }

    #endregion  Public Properties
  }
}
