namespace Cyotek.Data.Nbt
{
  public class TagShort : Tag
  {
    #region Public Constructors

    public TagShort()
      : this(string.Empty, 0)
    { }

    public TagShort(string name)
      : this(name, 0)
    { }

    public TagShort(short value)
      : this(string.Empty, value)
    { }

    public TagShort(string name, short value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.Short; }
    }

    #endregion

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return $"{indentString}[Short: {this.Name}={this.Value}]";
    }

    #endregion

    #region Public Properties

    public new short Value
    {
      get { return (short)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
