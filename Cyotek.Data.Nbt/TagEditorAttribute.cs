using System;

namespace Cyotek.Data.Nbt
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class TagEditorAttribute : Attribute
  {
    #region Instance Fields

    private readonly string _editorTypeName;

    #endregion

    #region Public Constructors

    public TagEditorAttribute(string editorTypeName)
    {
      _editorTypeName = editorTypeName;
    }

    #endregion

    #region Public Properties

    public string EditorTypeName
    {
      get { return _editorTypeName; }
    }

    #endregion
  }
}
