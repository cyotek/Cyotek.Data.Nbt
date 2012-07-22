using System;
using System.IO;

namespace Cyotek.Data.Nbt
{
  public interface ITag
  {
    #region  Events

    event EventHandler NameChanged;

    event EventHandler ParentChanged;

    event EventHandler ValueChanged;

    #endregion  Events

    #region  Private Properties

    bool CanRemove { get; }

    string FullPath { get; }

    string Name { get; set; }

    ITag Parent { get; set; }

    TagType Type { get; }

    object Value { get; set; }

    #endregion  Private Properties

    #region  Private Methods

    ITag[] Flatten();

    ITag[] GetAncestors();

    void Remove();

    string ToString(string indent);

    string ToValueString();

    void Write(Stream stream);

    void WriteUnnamed(Stream stream);

    #endregion  Private Methods
  }
}
