using A1_AutoDetail.App.Persistence;
using A1_AutoDetail.App.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace A1_AutoDetail.BulkBook
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Make sure we can find the database; exit if not.
            var dbPath = GetDbPath();
            if (!File.Exists(dbPath))
            {
                Console.Error.WriteLine("ERROR: Could not find file '" + dbPath + "'");
                Environment.ExitCode = 1; // Sets exit code
                return; // Exits the application
            }

            // 1) Register DbContext 
            var services = new ServiceCollection();
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlite("Data Source=" + dbPath);
            });

            // 2) Register required services and get a service provider
            services.AddScoped<IBookingService, BookingService>();
            using var provider = services.BuildServiceProvider();

            // 3) Create a scope (DbContext is scoped), get the booking service, and process the bookings
            using var scope = provider.CreateScope();
            var bookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();
            ProcessBookings(bookingService);

            /*
             * Important:
             *   In a console app, *we* are responsible for managing dependency lifetimes.
             *
             *   - The ServiceProvider represents the lifetime of the application host.
             *     It may own disposable infrastructure resources (for example, those used by EF Core).
             *
             *   - The scope represents a logical "request" boundary (similar to a single web request).
             *     All scoped services (including DbContext) live inside this scope.
             *
             *   Using the keyword 'using' on BOTH ensures that:
             *     - DbContext is disposed when the scope ends
             *     - Any resources owned by the provider are also released 
             *
             *   In an ASP.NET Core web app, the framework does this automatically.
             *   In a console app, we must do it explicitly.
             */

        } // end of Main

        static void ProcessBookings(IBookingService bookingService)
        {
            // Example call (replace with your loop over input lines)
            var nowUtc = DateTime.UtcNow;
            var result = bookingService.Book(customerId: 3, detailServiceId: 1, timeSlotId: 1, nowUtc: nowUtc);

            Console.WriteLine(result.Message);
        }


        // Get the path to the database file.
        // Use the same path as the web app. 
        static string GetDbPath()
        {
            // Go up from:
            //   A1/src/A1_AutoDetail.BulkBook/bin/Debug/net8.0/
            // to:
            //   A1/ 
            var baseDir = AppContext.BaseDirectory;
            var solutionRoot = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", ".."));
            var dbPath = Path.Combine(solutionRoot, "AppData", "a1-seed.db");

            return dbPath;
        }

    } // end class Program
} // end namespace A1_AutoDetail.BulkBook
