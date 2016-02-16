using System;

namespace Cyotek.Data.Nbt.Serialization
{
  [Flags]
  public enum ReadTagOptions
  {
    None = 0,

    IgnoreName = 1,

    IgnoreValue = 8
  }
}
