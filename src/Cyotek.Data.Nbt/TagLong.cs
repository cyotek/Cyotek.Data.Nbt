using System;
using System.Globalization;

namespace Cyotek.Data.Nbt
{
  public sealed class TagLong : Tag
  {
    #region Fields

    private long _value;

    #endregion

    #region Constructors

    public TagLong()
      : this(string.Empty, 0)
    { }

    public TagLong(string name)
      : this(name, 0)
    { }

    public TagLong(long value)
      : this(string.Empty, value)
    { }

    public TagLong(string name, long value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Long; }
    }

    public long Value
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
      this.Value = Convert.ToInt64(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[Long: {this.Name}={this.Value}]";
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
