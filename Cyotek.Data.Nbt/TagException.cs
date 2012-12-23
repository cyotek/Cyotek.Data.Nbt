using System;

namespace Cyotek.Data.Nbt
{
  public class TagException
    : Exception
  {
    #region Public Constructors

    public TagException()
      : base()
    {
    }

    public TagException(string message)
      : base(message)
    {
    }

    public TagException(string message, Exception innerException)
      : base(message, innerException)
    {
    }

    #endregion Public Constructors
  }
}