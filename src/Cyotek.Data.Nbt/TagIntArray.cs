using System.ComponentModel;
using System.Globalization;
using System.Linq;

namespace Cyotek.Data.Nbt
{
  public sealed class TagIntArray : Tag
  {
    #region Fields

    private int[] _value;

    #endregion

    #region Constructors

    public TagIntArray()
      : this(string.Empty, new int[0])
    { }

    public TagIntArray(string name)
      : this(name, new int[0])
    { }

    public TagIntArray(int[] value)
      : this(string.Empty, value)
    { }

    public TagIntArray(string name, int[] value)
      : base(name)
    {
      _value = value;
    }

    #endregion

    #region Properties

    public override TagType Type
    {
      get { return TagType.IntArray; }
    }

    [Category("Data")]
    [DefaultValue(typeof(int[]), null)]
    public int[] Value
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
      _value = (int[])value;
    }

    public override string ToString()
    {
      return $"[IntArray: {this.Name}={_value?.Length ?? 0} values]";
    }

    public override string ToValueString()
    {
      return string.Join(", ", this.Value.Select(b => b.ToString(CultureInfo.InvariantCulture)).ToArray());
    }

    #endregion
  }
}
