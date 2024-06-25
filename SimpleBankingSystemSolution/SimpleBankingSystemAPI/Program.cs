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
using System.Net;
using System.Net.Sockets;
using System.Text;
using WatchDog;
using WatchDog.src.Models;

namespace SimpleBankingSystemAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);

            #region Controllers
            // Add services to the container. 
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            #endregion

            #region Cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000", "http://192.168.0.103:3000")
                               .AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowCredentials();
                    });
            });
            #endregion

            #region Swagger
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
            #endregion

            #region Logging
            builder.Services.AddLogging(l => l.AddLog4Net());
            //WatchDog - https://www.youtube.com/watch?v=LO-QKMHK5ps&ab_channel=MohamadLawand
            builder.Services.AddWatchDogServices(opt =>
            {
                opt.SetExternalDbConnString = builder.Configuration.GetConnectionString("WatchDogConnection");
                opt.DbDriverOption = WatchDog.src.Enums.WatchDogDbDriverEnum.MSSQL;
            });

            #endregion

            #region Database
            builder.Services.AddDbContext<BankingContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            #endregion

            #region Injection (Repositories and Services)
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
            #endregion

            #region AutoMapper
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            #endregion

            #region Authentication
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

            #region Configuring PipeLines
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

            app.UseCors("AllowSpecificOrigin");

            app.MapControllers();

            // inject middleware for WatchDog
            app.UseWatchDogExceptionLogger();

            var watchdogCredentials = builder.Configuration.GetSection("WatchDog");
            app.UseWatchDog(opt =>
            {
                opt.WatchPageUsername = watchdogCredentials["username"];
                opt.WatchPagePassword = watchdogCredentials["password"];
            });
            #endregion

            #region Run
            //Setup local IP\
            static string LocalIPAddress()
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
                {
                    socket.Connect("8.8.8.8", 65530);
                    IPEndPoint? endPoint = socket.LocalEndPoint as IPEndPoint;
                    if (endPoint != null)
                    {
                        return endPoint.Address.ToString();
                    }
                    else
                    {
                        return "127.0.0.1";
                    }
                }
            }
            string localIP = LocalIPAddress();
            app.Urls.Add("http://" + localIP + ":5072");
            app.Urls.Add("https://" + localIP + ":7072");
            app.Run();
            #endregion
        }
    }
}
