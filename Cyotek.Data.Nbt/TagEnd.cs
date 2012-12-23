namespace Cyotek.Data.Nbt
{
  public class TagEnd
    : Tag
  {
    #region Public Constructors

    public TagEnd()
    {
    }

    #endregion Public Constructors

    #region Overriden Properties

    public override string Name { get { return string.Empty; } set { } }

    public override TagType Type
    {
      get { return TagType.End; }
    }

    #endregion Overriden Properties

    #region Public Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[End]", indentString);
    }

    #endregion Public Overridden Methods
  }
}