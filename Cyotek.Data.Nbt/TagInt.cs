namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt")]
  public class TagInt : Tag
  {
    #region Public Constructors

    public TagInt()
      : this(string.Empty, 0)
    { }

    public TagInt(string name)
      : this(name, 0)
    { }

    public TagInt(int value)
      : this(string.Empty, value)
    { }

    public TagInt(string name, int value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.Int; }
    }

    #endregion

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Int: {1}={2}]", indentString, Name, Value);
    }

    #endregion

    #region Public Properties

    public new int Value
    {
      get { return (int)base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
