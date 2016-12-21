namespace Cyotek.Data.Nbt
{
  public static partial class TagFactory
  {
    #region Static Methods

    public static ITag CreateTag(TagType tagType)
    {
      return CreateTag(tagType, null);
    }

    public static ITag CreateTag(TagType tagType, object value)
    {
      return CreateTag(tagType, string.Empty, value);
    }

    #endregion
  }
}
