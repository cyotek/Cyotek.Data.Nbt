using System;

namespace Cyotek.Data.Nbt
{
  public interface ITag
  {
    event EventHandler NameChanged;

    event EventHandler ParentChanged;

    event EventHandler ValueChanged;

    bool CanRemove { get; }

    string FullPath { get; }

    string Name { get; set; }

    ITag Parent { get; set; }

    TagType Type { get; }

    object Value { get; set; }

    ITag[] Flatten();

    ITag[] GetAncestors();

    void Remove();

    string ToString();

    string ToString(string indent);

    string ToValueString();
  }
}