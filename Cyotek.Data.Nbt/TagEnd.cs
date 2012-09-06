namespace Cyotek.Data.Nbt
{
  public class TagEnd
    : Tag
  {
    public TagEnd() { }

    public override string Name { get { return string.Empty; } set { } }

    public override TagType Type
    {
      get { return TagType.End; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[End]", indentString);
    }
  }
}