using System;
using System.ComponentModel;
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
      _value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.Float; }
    }

    [Category("Data")]
    [DefaultValue(0F)]
    public float Value
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
      _value = Convert.ToSingle(value);
    }

    public override string ToValueString()
    {
      return _value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
  }
}
