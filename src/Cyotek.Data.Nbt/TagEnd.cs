namespace Cyotek.Data.Nbt
{
  public class TagEnd : Tag
  {
    #region Properties

    public override string Name
    {
      get { return string.Empty; }
      set { }
    }

    public override TagType Type
    {
      get { return TagType.End; }
    }

    public override object Value
    {
      get { return null; }
      set { }
    }

    #endregion

    #region Methods

    public override string ToString(string indentString)
    {
      return $"{indentString}[End]";
    }

    #endregion
  }
}
