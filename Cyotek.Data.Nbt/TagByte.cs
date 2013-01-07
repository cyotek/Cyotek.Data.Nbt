namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
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

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.Byte; }
    }

    #endregion

    #region Overridden Members

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Byte: {1}={2}]", indentString, Name, Value);
    }

    #endregion

    #region Properties

    public new byte Value
    {
      get { return (byte)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
