namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagTextEditor, Cyotek.Windows.Forms.Nbt")]
  public class TagString : Tag
  {
    #region Public Constructors

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

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[String: {1}=\"{2}\"]", indentString, Name, Value);
    }

    #endregion

    #region Public Properties

    public new string Value
    {
      get { return (string)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
