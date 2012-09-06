namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagTextEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagString
    : Tag
  {
    public TagString()
      : this(string.Empty, string.Empty)
    { }

    public TagString(string name)
      : this(name, string.Empty)
    { }

    public TagString(string name, string value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override TagType Type
    {
      get { return TagType.String; }
    }

    public new string Value
    {
      get { return (string)base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[String: {1}=\"{2}\"]", indentString, Name, Value);
    }
  }
}