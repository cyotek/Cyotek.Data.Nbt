using System.IO;

namespace Cyotek.Data.Nbt
{
  public class TagEnd : Tag
  {
    public static TagEnd Singleton = new TagEnd();

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

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
    }

    public override void WriteUnnamed(Stream output) { }
  }
}