namespace Cyotek.Data.Nbt
{
  public interface ITagEditor
  {
    #region Members

    void Bind(ITag tag);

    void SaveChanges();

    #endregion
  }
}
