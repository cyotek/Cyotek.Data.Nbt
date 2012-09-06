namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagByteArrayEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagByteArray
    : Tag
  {
    public TagByteArray()
      : this(string.Empty, new byte[0])
    { }

    public TagByteArray(string name)
      : this(name, new byte[0])
    { }

    public TagByteArray(byte[] value)
      : this(string.Empty, value)
    { }

    public TagByteArray(string name, byte[] value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.ByteArray; }
    }

    public new byte[] Value
    {
      get { return (byte[])base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[ByteArray: {1}={2} values]", indentString, this.Name, (this.Value != null) ? this.Value.Length : 0);
    }
  }
}