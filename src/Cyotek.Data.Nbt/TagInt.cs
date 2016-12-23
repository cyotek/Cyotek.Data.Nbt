using System;
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
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Int; }
    }

    public int Value
    {
      get { return _value; }
      set
      {
        if (_value != value)
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
      this.Value = Convert.ToInt32(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[Int: {this.Name}={this.Value}]";
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
