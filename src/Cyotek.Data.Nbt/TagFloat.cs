using System;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagFloat : Tag
  {
    #region Fields

    private float _value;

    #endregion

    #region Constructors

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
      : base(name)
    {
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Float; }
    }

    public float Value
    {
      get { return _value; }
      set
      {
        if (Math.Abs(_value - value) > float.Epsilon)
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
      this.Value = Convert.ToSingle(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[Float: {this.Name}={this.Value}]";
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
