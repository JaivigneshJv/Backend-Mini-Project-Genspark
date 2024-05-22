﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SimpleBankingSystemAPI.Contexts;

#nullable disable

namespace SimpleBankingSystemAPI.Migrations
{
    [DbContext(typeof(BankingContext))]
    partial class BankingContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.30")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Account", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AccountType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Loan", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("AppliedDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PendingAmount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("RepaidDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("TargetDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.ToTable("Loans");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.LoanRepayment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<Guid>("LoanId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("LoanId");

                    b.ToTable("LoanRepayments");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("AccountId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsRecurring")
                        .HasColumnType("bit");

                    b.Property<Guid>("ReceiverId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("TransactionTypeId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("ReceiverId");

                    b.HasIndex("TransactionTypeId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.TransactionType", b =>
                {
                    b.Property<Guid>("TransactionTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("RaiseRequest")
                        .HasColumnType("bit");

                    b.Property<string>("TypeName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TransactionTypeId");

                    b.ToTable("TransactionTypes");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Contact")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("94391507-55db-4141-9326-e2f6e86f8bca"),
                            Contact = "1234567890",
                            CreatedDate = new DateTime(2024, 5, 22, 10, 18, 38, 355, DateTimeKind.Utc).AddTicks(8246),
                            DateOfBirth = new DateTime(2002, 10, 11, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Email = "adminuser@simplebank.com",
                            FirstName = "Admin",
                            IsActive = true,
                            LastName = "User",
                            PasswordHash = new byte[] { 130, 80, 163, 66, 45, 102, 65, 225, 99, 14, 157, 246, 161, 132, 236, 182, 254, 32, 75, 232, 98, 95, 3, 222, 205, 183, 43, 244, 40, 220, 42, 212, 51, 56, 91, 200, 44, 216, 104, 185, 86, 11, 159, 134, 67, 201, 143, 192, 227, 135, 136, 4, 205, 1, 120, 47, 34, 133, 179, 223, 180, 29, 4, 133, 119, 96, 153, 95, 194, 151, 185, 31, 229, 151, 65, 208, 222, 149, 10, 3, 87, 191, 174, 112, 147, 201, 206, 210, 38, 164, 232, 53, 137, 142, 233, 77, 80, 185, 9, 157, 21, 187, 207, 27, 83, 228, 54, 112, 87, 184, 32, 104, 21, 64, 147, 58, 245, 99, 7, 62, 96, 224, 204, 178, 5, 196, 235, 121 },
                            PasswordSalt = new byte[] { 120, 222, 58, 171, 231, 203, 20, 32, 44, 93, 161, 179, 142, 133, 98, 184, 77, 107, 133, 251, 67, 252, 251, 52, 140, 222, 238, 85, 151, 205, 39, 116, 0, 164, 152, 110, 66, 188, 57, 202, 208, 83, 131, 139, 68, 61, 177, 245, 158, 66, 54, 144, 25, 20, 115, 204, 9, 85, 175, 50, 123, 93, 248, 231 },
                            Role = "Admin",
                            UpdatedDate = new DateTime(2024, 5, 22, 10, 18, 38, 355, DateTimeKind.Utc).AddTicks(8248),
                            Username = "Admin"
                        });
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Account", b =>
                {
                    b.HasOne("SimpleBankingSystemAPI.Models.User", "User")
                        .WithMany("Accounts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Loan", b =>
                {
                    b.HasOne("SimpleBankingSystemAPI.Models.Account", "Account")
                        .WithMany("Loans")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.LoanRepayment", b =>
                {
                    b.HasOne("SimpleBankingSystemAPI.Models.Loan", "Loan")
                        .WithMany("LoanRepayments")
                        .HasForeignKey("LoanId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Loan");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Transaction", b =>
                {
                    b.HasOne("SimpleBankingSystemAPI.Models.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SimpleBankingSystemAPI.Models.Account", "Receiver")
                        .WithMany()
                        .HasForeignKey("ReceiverId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SimpleBankingSystemAPI.Models.TransactionType", "TransactionType")
                        .WithMany("Transactions")
                        .HasForeignKey("TransactionTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Receiver");

                    b.Navigation("TransactionType");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Account", b =>
                {
                    b.Navigation("Loans");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.Loan", b =>
                {
                    b.Navigation("LoanRepayments");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.TransactionType", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("SimpleBankingSystemAPI.Models.User", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
