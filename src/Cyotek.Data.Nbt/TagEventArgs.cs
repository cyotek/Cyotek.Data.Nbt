using System;

namespace Cyotek.Data.Nbt
{
  public class TagEventArgs : EventArgs
  {
    #region Constructors

    public TagEventArgs(Tag tag)
      : this()
    {
      this.Tag = tag;
    }

    protected TagEventArgs()
    { }

    #endregion

    #region Properties

    public Tag Tag { get; protected set; }

    #endregion
  }
}
