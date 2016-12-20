using System;
using System.Linq;

namespace Cyotek.Data.Nbt
{
  public sealed class TagByteArray : Tag
  {
    #region Fields

    private byte[] _value;

    #endregion

    #region Constructors

    public TagByteArray()
      : this(string.Empty, new byte[0])
    { }

    public TagByteArray(string name)
      : this(name, new byte[0])
    { }

    public TagByteArray(byte[] value)
      : this(string.Empty, value)
    { }

    public TagByteArray(string name, byte[] value)
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.ByteArray; }
    }

    public byte[] Value
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
      this.Value = (byte[])value;
    }

    public override string ToString(string indentString)
    {
      return $"{indentString}[ByteArray: {this.Name}={this.Value?.Length ?? 0} values]";
    }

    public override string ToValueString()
    {
      return string.Join(", ", this.Value.Select(b => b.ToString("X2")).ToArray());
    }

    #endregion
  }
}
