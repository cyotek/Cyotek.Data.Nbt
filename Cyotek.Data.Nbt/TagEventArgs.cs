using System;

namespace Cyotek.Data.Nbt
{
  public class TagEventArgs : EventArgs
  {
    #region Constructors

    public TagEventArgs(ITag tag)
      : this()
    {
      this.Tag = tag;
    }

    protected TagEventArgs()
    { }

    #endregion

    #region Properties

    public ITag Tag { get; protected set; }

    #endregion
  }
}
