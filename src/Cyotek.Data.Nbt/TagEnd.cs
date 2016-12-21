using System;

namespace Cyotek.Data.Nbt
{
  public sealed class TagEnd : Tag
  {
    #region Properties

    public override string Name
    {
      get { return string.Empty; }
      set { }
    }

    public override TagType Type
    {
      get { return TagType.End; }
    }

    #endregion

    #region Methods

    public override object GetValue()
    {
      throw new NotSupportedException("Tag does not support values.");
    }

    public override void SetValue(object value)
    {
      throw new NotSupportedException("Tag does not support values.");
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[End]";
    }

    public override string ToValueString()
    {
      return string.Empty;
    }

    #endregion
  }
}
