using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SimpleBankingSystemAPI.Contexts;
using SimpleBankingSystemAPI.Interfaces.Repositories;
using SimpleBankingSystemAPI.Interfaces.Services;
using SimpleBankingSystemAPI.Mappings;
using SimpleBankingSystemAPI.Models;
using SimpleBankingSystemAPI.Repositories;
using SimpleBankingSystemAPI.Services;
using System.Text;
using WatchDog;

namespace SimpleBankingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            #region ConfigureServices
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddLogging(l => l.AddLog4Net());
            builder.Services.AddSwaggerGen(option =>
            {
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement { { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, new string[] { } } });
            });

            builder.Services.AddWatchDogServices();

            builder.Services.AddDbContext<BankingContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IAccountRepository, AccountRepository>();
            builder.Services.AddScoped<IAccountService, AccountService>();

            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<ITransactionService, TransactionService>();

            builder.Services.AddScoped<ITransactionVerificationRepository, TransactionVerificationRepository>();

            builder.Services.AddScoped<IPendingAccountClosingRepository, PendingAccountClosingRepository>();

            builder.Services.AddScoped<IPendingAccountTransactionRepository, PendingAccountTransactionRepository>();

            builder.Services.AddScoped<ILoanRepository, LoanRepository>();
            builder.Services.AddScoped<ILoanServices, LoanService>();

            builder.Services.AddScoped<ILoanRepaymentRepository, LoanRepaymentRepository>();

            builder.Services.AddScoped<IEmailVerificationRepository, EmailVerificationRepository>();
            builder.Services.AddTransient<IEmailSender, EmailSenderService>();


            builder.Services.AddAutoMapper(typeof(MappingProfile));

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["TokenKey:JWT"])),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Cookies["jwt-token-banking-app"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        }
                    };
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            #endregion


            #region ConfigurePipeline

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
