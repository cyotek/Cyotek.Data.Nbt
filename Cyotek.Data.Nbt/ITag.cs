using System;

namespace Cyotek.Data.Nbt
{
  public interface ITag
  {
    #region Events

    event EventHandler NameChanged;

    event EventHandler ParentChanged;

    event EventHandler ValueChanged;

    #endregion

    #region Properties

    bool CanRemove { get; }

    string FullPath { get; }

    string Name { get; set; }

    ITag Parent { get; set; }

    TagType Type { get; }

    object Value { get; set; }

    #endregion

    #region Methods

    ITag[] Flatten();

    ITag[] GetAncestors();

    byte[] GetValue();

    void Remove();

    string ToString();

    string ToString(string indent);

    string ToValueString();

    #endregion
  }
}
