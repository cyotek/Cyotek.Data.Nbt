namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagLong : Tag
  {
    #region Constructors

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

    #region Overridden Members

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Long: {1}={2}]", indentString, Name, Value);
    }

    #endregion

    #region Properties

    public new long Value
    {
      get { return (long)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
