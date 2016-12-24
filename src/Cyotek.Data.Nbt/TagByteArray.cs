using System.ComponentModel;
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
      : base(name)
    {
      _value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.ByteArray; }
    }

    [Category("Data")]
    [DefaultValue(typeof(byte[]), null)]
    public byte[] Value
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
      _value = (byte[])value;
    }

    public override string ToString()
    {
      return $"[ByteArray: {this.Name}={_value?.Length ?? 0} values]";
    }

    public override string ToValueString()
    {
      return string.Join(", ", this.Value.Select(b => b.ToString("X2")).ToArray());
    }

    #endregion
  }
}
