using System.Linq;

namespace Cyotek.Data.Nbt
{
  public sealed partial class TagByteArray
  {
    #region Methods

    public override string ToValueString()
    {
      return string.Join(", ", this.Value.Select(b => b.ToString("X2")).ToArray());
    }

    #endregion
  }
}
