namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagNumberEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagFloat
    : Tag
  {
    #region Public Constructors

    public TagFloat()
      : this(string.Empty, 0)
    { }

    public TagFloat(string name)
      : this(name, 0)
    { }

    public TagFloat(float value)
      : this(string.Empty, value)
    { }

    public TagFloat(string name, float value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion Public Constructors

    #region Overriden Properties

    public override TagType Type
    {
      get { return TagType.Float; }
    }

    #endregion Overriden Properties

    #region Public Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[Float: {1}={2}]", indentString, Name, Value);
    }

    #endregion Public Overridden Methods

    #region Public Properties

    public new float Value
    {
      get { return (float)base.Value; }
      set { base.Value = value; }
    }

    #endregion Public Properties
  }
}