namespace Cyotek.Data.Nbt
{
  public class TagByte : Tag
  {
    #region Constructors

    public TagByte()
      : this(string.Empty, 0)
    { }

    public TagByte(string name)
      : this(name, 0)
    { }

    public TagByte(byte value)
      : this(string.Empty, value)
    { }

    public TagByte(string name, byte value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Byte; }
    }

    public new byte Value
    {
      get { return (byte)base.Value; }
      set { base.Value = value; }
    }

    #endregion

    #region Methods

    public override string ToString(string indentString)
    {
      return $"{indentString}[Byte: {this.Name}={this.Value}]";
    }

    #endregion
  }
}
