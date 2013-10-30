using System;

namespace Cyotek.Data.Nbt
{
  [Flags]
  public enum NbtOptions
  {
    None = 0,

    ReadHeader = 1,

    Compress = 2,

    SingleUse = 4,

    HeaderOnly = 8
  }
}
