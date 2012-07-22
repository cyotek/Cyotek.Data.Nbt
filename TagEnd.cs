using System.IO;

namespace Cyotek.Data.Nbt
{
  public class TagEnd : Tag
  {
    #region  Public Class Member Declarations

    public static TagEnd Singleton = new TagEnd();

    #endregion  Public Class Member Declarations

    #region  Public Constructors

    public TagEnd() { }

    #endregion  Public Constructors

    #region  Overriden Properties

    public override string Name { get { return string.Empty; } set { } }

    public override TagType Type
    {
      get { return TagType.End; }
    }

    #endregion  Overriden Properties

    #region  Public Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[End]", indentString);
    }

    public override void Write(Stream output)
    {
      output.WriteByte((byte)Type);
    }

    public override void WriteUnnamed(Stream output) { }

    #endregion  Public Overridden Methods
  }
}
