using System;

namespace Cyotek.Data.Nbt
{
  public sealed class TagString : Tag
  {
    #region Fields

    private string _value;

    #endregion

    #region Constructors

    public TagString()
      : this(string.Empty, string.Empty)
    { }

    public TagString(string name)
      : this(name, string.Empty)
    { }

    public TagString(string name, string value)
      : base(name)
    {
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.String; }
    }

    public string Value
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
      this.Value = Convert.ToString(value);
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[String: {this.Name}=\"{this.Value}\"]";
    }

    public override string ToValueString()
    {
      return _value ?? string.Empty;
    }

    #endregion
  }
}
