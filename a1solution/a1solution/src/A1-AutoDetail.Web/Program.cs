using A1_AutoDetail.App.Persistence;
using A1_AutoDetail.App.Services;
using A1_AutoDetail.App.Domain.Contracts;
using A1_AutoDetail.App.Persistence.Dao;
using Microsoft.EntityFrameworkCore;

namespace A1_AutoDetail
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // Inject the Service Layer classes. These may be used by the Controller.
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IBookingDataAdapter, BookingDataAdapter>();
            builder.Services.AddScoped<IBookingDao, BookingDao>();

            // Inject the DbContext class.
            // This should be used by the Service layer, NOT by the Controller.
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlite(builder.Configuration.GetConnectionString("AppDb"))
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
