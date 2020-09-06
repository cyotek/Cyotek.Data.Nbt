using System.IO;

namespace Cyotek.Data.Nbt.Tests.Serialization
{
  internal sealed class NoSeekStream : MemoryStream
  {
    #region Constructors

    public NoSeekStream(byte[] buffer)
      : base(buffer)
    { }

    #endregion

    #region Properties

    public override bool CanSeek
    {
      get { return false; }
    }

    #endregion
  }
}
