namespace Cyotek.Data.Nbt
{
  public class TagDouble : Tag
  {
    #region Constructors

    public TagDouble()
      : this(string.Empty, 0)
    { }

    public TagDouble(string name)
      : this(name, 0)
    { }

    public TagDouble(double value)
      : this(string.Empty, value)
    { }

    public TagDouble(string name, double value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Double; }
    }

    public new double Value
    {
      get { return (double)base.Value; }
      set { base.Value = value; }
    }

    #endregion

    #region Methods

    public override string ToString(string indentString)
    {
      return $"{indentString}[Double: {this.Name}={this.Value}]";
    }

    #endregion
  }
}
