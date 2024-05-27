using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using System;

namespace SimpleBankingSystemUnitTest.Repository
{
    public class PendingAccountClosingRepositoryTest
    {
        BankingContext context;
        IPendingAccountClosingRepository pendingAccountClosingRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyPendingAccountClosingRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            pendingAccountClosingRepo = new PendingAccountClosingRepository(context);
        }

        [Test, Order(1)]
        public void Constructor_RepositoryInstanceCreated()
        {
            // Assert
            Assert.IsNotNull(pendingAccountClosingRepo);
        }
    }
}
