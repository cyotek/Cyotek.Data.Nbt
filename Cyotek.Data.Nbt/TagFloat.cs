namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagFloat
    : Tag
  {
    public TagFloat()
      : this(string.Empty, 0)
    { }

    public TagFloat(string name)
      : this(name, 0)
    { }

    public TagFloat(float value)
      : this(string.Empty, value)
    { }

    public TagFloat(string name, float value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.Float; }
    }

    public new float Value
    {
      get { return (float)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Float: {1}={2}]", indentString, Name, Value);
    }
  }
}