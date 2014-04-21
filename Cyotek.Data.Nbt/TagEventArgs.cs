using System;

namespace Cyotek.Data.Nbt
{
  public class TagEventArgs : EventArgs
  {
    #region Public Constructors

    public TagEventArgs(ITag tag)
      : this()
    {
      this.Tag = tag;
    }

    #endregion

    #region Protected Constructors

    protected TagEventArgs()
    { }

    #endregion

    #region Public Properties

    public ITag Tag { get; protected set; }

    #endregion
  }
}
