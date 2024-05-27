using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Interfaces.Repositories
{
    public interface IPendingAccountClosingRepository : IRepository<Guid, PendingAccountClosing>
    {
    }
}
