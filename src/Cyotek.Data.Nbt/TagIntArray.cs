using System.Globalization;
using System.Linq;

namespace Cyotek.Data.Nbt
{
  public sealed partial class TagIntArray
  {
    #region Methods

    public override string ToValueString()
    {
      return string.Join(", ", this.Value.Select(b => b.ToString(CultureInfo.InvariantCulture)).ToArray());
    }

    #endregion
  }
}
