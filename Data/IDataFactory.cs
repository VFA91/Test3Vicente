using Data.Model;

namespace Data
{
    public interface IDataFactory
    {
        EventManagementEntities GetMainContext();
    }
}
