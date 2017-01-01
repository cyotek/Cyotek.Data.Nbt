using System;

namespace Cyotek.Data.Nbt.Tests
{
  internal sealed class BadTag : Tag
  {
    #region Constructors

    public BadTag(string name)
      : base(name)
    { }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return (TagType)100; }
    }

    #endregion

    #region Methods

    public override object GetValue()
    {
      throw new NotImplementedException();
    }

    public override void SetValue(object value)
    {
      throw new NotImplementedException();
    }

    public override string ToValueString()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
