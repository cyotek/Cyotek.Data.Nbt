namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagShort
    : Tag
  {
    public TagShort()
      : this(string.Empty, 0)
    { }

    public TagShort(string name)
      : this(name, 0)
    { }

    public TagShort(short value)
      : this(string.Empty, value)
    { }

    public TagShort(string name, short value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.Short; }
    }

    public new short Value
    {
      get { return (short)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Short: {1}={2}]", indentString, Name, Value);
    }
  }
}