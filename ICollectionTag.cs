using System.Collections.Generic;

namespace Cyotek.Data.Nbt
{
  public interface ICollectionTag
  {
    #region  Private Properties

    bool IsList { get; }

    TagType LimitToType { get; }

    IList<ITag> Values { get; }

    #endregion  Private Properties
  }
}
