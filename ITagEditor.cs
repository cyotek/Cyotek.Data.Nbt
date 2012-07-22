
namespace Cyotek.Data.Nbt
{
  public interface ITagEditor
  {
    #region  Private Methods

    void Bind(ITag tag);

    void SaveChanges();

    #endregion  Private Methods
  }
}
