namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagDouble
    : Tag
  {
    public TagDouble()
      : this(string.Empty, 0)
    { }

    public TagDouble(string name)
      : this(name, 0)
    { }

    public TagDouble(double value)
      : this(string.Empty, value)
    { }

    public TagDouble(string name, double value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.Double; }
    }

    public new double Value
    {
      get { return (double)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Double: {1}={2}]", indentString, Name, Value);
    }
  }
}