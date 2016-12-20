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

    #endregion

    #region Methods

    ITag[] Flatten();

    ITag[] GetAncestors();

    object GetValue();

    void Remove();

    void SetValue(object value);

    string ToString();

    string ToString(string indent);

    string ToValueString();

    #endregion
  }
}
