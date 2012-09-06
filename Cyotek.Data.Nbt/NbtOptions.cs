using System;

namespace Cyotek.Data.Nbt
{
  [Flags]
  public enum NbtOptions
  {
    None = 0,
    Header = 1,
    Compress = 2
  }
}