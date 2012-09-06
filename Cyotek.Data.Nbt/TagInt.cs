namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagInt
    : Tag
  {
    public TagInt()
      : this(string.Empty, 0)
    { }

    public TagInt(string name)
      : this(name, 0)
    { }

    public TagInt(int value)
      : this(string.Empty, value)
    { }

    public TagInt(string name, int value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.Int; }
    }

    public new int Value
    {
      get { return (int)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Int: {1}={2}]", indentString, Name, Value);
    }
  }
}