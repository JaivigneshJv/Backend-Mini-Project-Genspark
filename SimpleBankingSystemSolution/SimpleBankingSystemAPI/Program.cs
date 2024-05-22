using Microsoft.EntityFrameworkCore;
using SimpleBankingSystemAPI.Contexts;

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
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<BankingContext>(options =>
             options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
            #endregion
        }
    }
}
