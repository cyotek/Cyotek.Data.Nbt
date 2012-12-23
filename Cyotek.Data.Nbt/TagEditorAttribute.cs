using System;

namespace Cyotek.Data.Nbt
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class TagEditorAttribute
    : Attribute
  {
    #region Private Member Declarations

    private string _editorTypeName;

    #endregion Private Member Declarations

    #region Public Constructors

    public TagEditorAttribute(string editorTypeName)
    {
      _editorTypeName = editorTypeName;
    }

    #endregion Public Constructors

    #region Public Properties

    public string EditorTypeName
    { get { return _editorTypeName; } }

    #endregion Public Properties
  }
}