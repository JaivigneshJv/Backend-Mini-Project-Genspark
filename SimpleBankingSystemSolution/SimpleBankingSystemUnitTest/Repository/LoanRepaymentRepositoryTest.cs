using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using System;

namespace SimpleBankingSystemUnitTest.Repository
{
    public class LoanRepaymentRepositoryTest
    {
        BankingContext context;
        ILoanRepaymentRepository loanRepaymentRepo;

        [SetUp]
        public void SetUp()
        {
            DbContextOptionsBuilder<BankingContext> optionsBuilder = new DbContextOptionsBuilder<BankingContext>()
                                                            .UseInMemoryDatabase("dummyLoanRepaymentRepositoryDB");
            context = new BankingContext(optionsBuilder.Options);
            loanRepaymentRepo = new LoanRepaymentRepository(context);
        }

        [Test, Order(1)]
        public void Constructor_RepositoryInstanceCreated()
        {
            // Assert
            Assert.IsNotNull(loanRepaymentRepo);
        }
    }
}
