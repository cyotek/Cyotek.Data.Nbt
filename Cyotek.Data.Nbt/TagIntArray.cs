namespace Cyotek.Data.Nbt
{
  [TagEditor("Cyotek.Windows.Forms.Nbt.TagIntArrayEditor, Cyotek.Windows.Forms.Nbt, Version=1.0.0.0, Culture=neutral, PublicKeyToken=9d164292f52c48c9")]
  public class TagIntArray
    : Tag
  {
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

    public override TagType Type
    {
      get { return TagType.IntArray; }
    }

    public new int[] Value
    {
      get { return (int[])base.Value; }
      set { base.Value = value; }
    }

    public override string ToString(string indentString)
    {
      return string.Format("{0}[IntArray: {1}={2} values]", indentString, this.Name, (this.Value != null) ? this.Value.Length : 0);
    }
  }
}