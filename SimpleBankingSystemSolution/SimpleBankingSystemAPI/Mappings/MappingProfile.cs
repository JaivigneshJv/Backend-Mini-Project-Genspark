using AutoMapper;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Models.DTOs.AccountsDTOs;
using SimpleBankingSystemAPI.Models.DTOs.LoanDTOs;
using SimpleBankingSystemAPI.Models.DTOs.TransactionDTOs;
using SimpleBankingSystemAPI.Models.DTOs.UserDTOs;

namespace SimpleBankingSystemAPI.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserProfileDto>().ReverseMap();
            CreateMap<UpdateUserProfileRequest, User>();
            CreateMap<Account, OpenAccountRequest>();
            CreateMap<Account, AccountDto>();
            CreateMap<Transaction, TransactionDto>().ReverseMap();
            CreateMap<TransactionDto, TransactionVerification>().ReverseMap();
            CreateMap<TransactionDto, TransactionVerification>();
            CreateMap<Transaction, TransactionVerification>().ReverseMap();
            CreateMap<Account, AccountClosingDto>().ReverseMap();
            CreateMap<Transaction, TransactionRequestDto>();
            CreateMap<Transaction, TransactionRequestDto>().ReverseMap();
            CreateMap<PendingAccountTransaction, TransactionRequestDto>().ReverseMap();
            CreateMap<PendingAccountClosing, AccountClosingDto>().ReverseMap();
            CreateMap<LoanRequest, Loan>();
            CreateMap<LoanRequest, Loan>().ReverseMap();
            CreateMap<Loan, LoanDto>().ReverseMap(); 
            CreateMap<Loan, LoanDto>();
            CreateMap<LoanRepaymentDto, LoanRepayment>();
            CreateMap<LoanRepaymentDto, LoanRepayment>().ReverseMap();




        }
    }
}
