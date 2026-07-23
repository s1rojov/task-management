
using Microsoft.EntityFrameworkCore;
using Task_management.Data;
using Task_management.Repositories.StatusRepositories;
using Task_management.Repositories.UserRepositories;
using Task_management.Services.Statuses;
using Task_management.Services.Users;

namespace Task_management
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //services
            builder.Services.AddTransient<IUserRepository, UserRepository>();
            builder.Services.AddTransient<IUserService, UserService>();

            builder.Services.AddTransient<IStatusRepository, StatusRepository>();
            builder.Services.AddTransient<IStatusService, StatusService>();

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
