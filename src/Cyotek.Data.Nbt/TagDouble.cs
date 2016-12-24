using System;
using System.ComponentModel;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagDouble : Tag
  {
    #region Fields

    private double _value;

    #endregion

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
      : base(name)
    {
      _value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Double; }
    }

    [Category("Data")]
    [DefaultValue(0D)]
    public double Value
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
      _value = Convert.ToDouble(value);
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
