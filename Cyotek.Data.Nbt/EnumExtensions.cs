using System;

namespace Cyotek.Data.Nbt
{
  internal static class EnumExtensions
  {
    #region Public Class Methods

    public static bool HasFlag(this Enum value, object flag)
    {
      return (Convert.ToInt32(value) & (int)flag) == (int)flag;
    }

    #endregion Public Class Methods
  }
}