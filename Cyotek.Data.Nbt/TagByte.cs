namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagByte
    : Tag
  {
    public TagByte()
      : this(string.Empty, 0)
    { }

    public TagByte(string name)
      : this(name, 0)
    { }

    public TagByte(byte value)
      : this(string.Empty, value)
    { }

    public TagByte(string name, byte value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.Byte; }
    }

    public new byte Value
    {
      get { return (byte)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Byte: {1}={2}]", indentString, Name, Value);
    }
  }
}