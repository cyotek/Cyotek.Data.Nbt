using System;
using System.ComponentModel;
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

    [Category("Data")]
    [DefaultValue((byte)0)]
    public byte Value
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
      _value = Convert.ToByte(value);
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
