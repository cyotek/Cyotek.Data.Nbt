namespace Cyotek.Data.Nbt
{
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

    #region Properties

    public override TagType Type
    {
      get { return TagType.String; }
    }

    public new string Value
    {
      get { return (string)base.Value; }
      set { base.Value = value; }
    }

    #endregion

    #region Methods

    public override string ToString(string indentString)
    {
      return $"{indentString}[String: {this.Name}=\"{this.Value}\"]";
    }

    #endregion
  }
}
