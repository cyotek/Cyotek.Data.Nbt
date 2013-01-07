namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagTextEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagString : Tag
  {
    #region Constructors

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

    #endregion

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.String; }
    }

    #endregion

    #region Overridden Members

    public override string ToString(string indentString)
    {
      return string.Format("{0}[String: {1}=\"{2}\"]", indentString, Name, Value);
    }

    #endregion

    #region Properties

    public new string Value
    {
      get { return (string)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
