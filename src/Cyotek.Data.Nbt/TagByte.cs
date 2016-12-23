using System;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagByte : Tag
  {
    #region Fields

    private byte _value;

    #endregion

    #region Constructors

    public TagByte()
      : this(string.Empty, 0)
    { }

    public TagByte(string name)
      : this(name, 0)
    { }

    public TagByte(byte value)
      : this(string.Empty, value)
    { }

    public TagByte(string name, byte value)
      : base(name)
    {
      _value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Byte; }
    }

    public byte Value
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
      this.Value = Convert.ToByte(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[Byte: {this.Name}={this.Value}]";
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
