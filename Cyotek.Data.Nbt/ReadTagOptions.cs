using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyotek.Data.Nbt
{
  [Flags]
  public enum ReadTagOptions
  {
    None = 0,

    IgnoreName = 1,

    IgnoreValue = 8
  }
}
