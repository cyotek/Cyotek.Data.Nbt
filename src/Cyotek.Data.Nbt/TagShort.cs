using System;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagShort : Tag
  {
    #region Fields

    private short _value;

    #endregion

    #region Constructors

    public TagShort()
      : this(string.Empty, 0)
    { }

    public TagShort(string name)
      : this(name, 0)
    { }

    public TagShort(short value)
      : this(string.Empty, value)
    { }

    public TagShort(string name, short value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Short; }
    }

    public short Value
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
      this.Value = Convert.ToInt16(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[Short: {this.Name}={this.Value}]";
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
