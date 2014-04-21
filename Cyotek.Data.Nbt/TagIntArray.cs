using System.Globalization;
using System.Linq;

namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagIntArrayEditor, Cyotek.Windows.Forms.Nbt")]
  public class TagIntArray : Tag
  {
    #region Public Constructors

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
    {
      this.Name = name;
      this.Value = value;
    }

    #endregion

    #region Overridden Properties

    public override TagType Type
    {
      get { return TagType.IntArray; }
    }

    #endregion

    #region Overridden Methods

    public override string ToString(string indentString)
    {
      return string.Format("{0}[IntArray: {1}={2} values]", indentString, this.Name, (this.Value != null) ? this.Value.Length : 0);
    }

    public override string ToValueString()
    {
      return string.Join(", ", this.Value.Select(b => b.ToString(CultureInfo.InvariantCulture)).ToArray());
    }

    #endregion

    #region Public Properties

    public new int[] Value
    {
      get { return (int[])base.Value; }
      set { base.Value = value; }
    }

    #endregion
  }
}
