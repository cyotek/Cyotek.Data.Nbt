using System.Collections.Generic;

namespace Cyotek.Data.Nbt
{
  public interface ITagCollection
  {
    bool IsList { get; }

    TagType LimitToType { get; set; }

    IList<ITag> Values { get; }
  }
}