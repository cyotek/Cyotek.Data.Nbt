using System;

namespace Cyotek.Data.Nbt
{
  internal static class EnumExtensions
  {
    public static bool HasFlag(this Enum value, object flag)
    {
      return (Convert.ToInt32(value) & (int)flag) == (int)flag;
    }
  }
}