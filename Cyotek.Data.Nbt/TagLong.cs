namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagLong
    : Tag
  {
    public TagLong()
      : this(string.Empty, 0)
    { }

    public TagLong(string name)
      : this(name, 0)
    { }

    public TagLong(long value)
      : this(string.Empty, value)
    { }

    public TagLong(string name, long value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.Long; }
    }

    public new long Value
    {
      get { return (long)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Long: {1}={2}]", indentString, Name, Value);
    }
  }
}