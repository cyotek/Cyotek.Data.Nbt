namespace Cyotek.Data.Nbt
{
  public class TagLong : Tag
  {
    #region Public Constructors

    public TagLong()
      : this(string.Empty, 0)
    { }

    public TagLong(string name)
      : this(name, 0)
    { }

    public TagLong(long value)
      : this(string.Empty, value)
    { }

    public TagLong(string name, long value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.Long; }
    }

    #endregion

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return $"{indentString}[Long: {this.Name}={this.Value}]";
    }

    #endregion

    #region Public Properties

    public new long Value
    {
      get { return (long)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
