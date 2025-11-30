using shukuma.persistence.firebase;

namespace shukuma
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var config = builder.Configuration;

            // Firebase/Firestore connection
            builder.Services.ConnectFirestore(config);

            // Add session support
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2); // Session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Add services to the container
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseRouting();

            // Enable session
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            // Configure routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Profile}/{action=SignUp}/{id?}");

            app.Run();
        }
    }
}