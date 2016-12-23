using System;
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
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Double; }
    }

    public double Value
    {
      get { return _value; }
      set
      {
        if (Math.Abs(_value - value) > double.Epsilon)
        {
          _value = value;

          this.OnValueChanged(EventArgs.Empty);
        }
      }
    }

    #endregion

    #region Methods

    public override object GetValue()
    {
      return _value;
    }

    public override void SetValue(object value)
    {
      this.Value = Convert.ToDouble(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[Double: {this.Name}={this.Value}]";
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
