using System;

namespace Cyotek.Data.Nbt
{
  public class TagException
    : Exception
  {
    public TagException() : base() { }

    public TagException(string message) : base(message) { }

    public TagException(string message, Exception innerException) : base(message, innerException) { }
  }
}