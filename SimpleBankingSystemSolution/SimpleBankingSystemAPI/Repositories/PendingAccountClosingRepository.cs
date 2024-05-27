using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;

namespace SimpleBankingSystemAPI.Repositories
{
    public class PendingAccountClosingRepository : Repository<Guid, PendingAccountClosing>, IPendingAccountClosingRepository
    {
        private readonly BankingContext _context;
        public PendingAccountClosingRepository(BankingContext context) : base(context)
        {
            _context = context;
        }
    }
}
