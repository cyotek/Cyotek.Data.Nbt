using System;
using System.ComponentModel;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagInt : Tag
  {
    #region Fields

    private int _value;

    #endregion

    #region Constructors

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
      : base(name)
    {
      _value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Int; }
    }

    [Category("Data")]
    [DefaultValue(0)]
    public int Value
    {
      get { return _value; }
      set { _value = value; }
    }

    #endregion

    #region Methods

    public override object GetValue()
    {
      return _value;
    }

    public override void SetValue(object value)
    {
      _value = Convert.ToInt32(value);
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
