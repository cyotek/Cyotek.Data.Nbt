using System;

namespace Cyotek.Data.Nbt
{
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public sealed class TagEditorAttribute
    : Attribute
  {
    private string _editorTypeName;

    public TagEditorAttribute(string editorTypeName)
    {
      _editorTypeName = editorTypeName;
    }

    public string EditorTypeName
    { get { return _editorTypeName; } }
  }
}