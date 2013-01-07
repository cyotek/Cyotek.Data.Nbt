namespace Cyotek.Data.Nbt
{
  public class TagEnd : Tag
  {
    #region Overridden Properties

    public override string Name
    {
      get { return string.Empty; }
      set { }
    }

    public override TagType Type
    {
      get { return TagType.End; }
    }

    #endregion

    #region Overridden Members

    public override string ToString(string indentString)
    {
      return string.Format("{0}[End]", indentString);
    }

    #endregion
  }
}
