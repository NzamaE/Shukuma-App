using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using shukuma.domain.Models;
using shukuma.domain.Interfaces;
using Google.Apis.Auth.OAuth2;
using System.Text;

namespace shukuma.persistence.firebase;

public static class DependencyInjection
{
    public static IServiceCollection ConnectFirestore(this IServiceCollection services, IConfiguration configuration)
    {
        var fireStoreAppOptions = new FirestoreAppOptions();
        configuration.Bind(nameof(FirestoreAppOptions), fireStoreAppOptions);

        Console.WriteLine($"=== FIREBASE SETUP ===");
        Console.WriteLine($"Project ID: {fireStoreAppOptions.ProjectId}");

        // Try to get credentials from environment variable first (for production/Render)
        var firebaseCredentialsJson = Environment.GetEnvironmentVariable("FIREBASE_CREDENTIALS");
        
        if (!string.IsNullOrEmpty(firebaseCredentialsJson))
        {
            Console.WriteLine("Using Firebase credentials from environment variable");
            
            // Write credentials to a temporary file for Google Cloud SDK
            var tempPath = Path.Combine(Path.GetTempPath(), $"{fireStoreAppOptions.ProjectId}.json");
            File.WriteAllText(tempPath, firebaseCredentialsJson);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", tempPath);
            
            Console.WriteLine("✅ Firebase credentials loaded from environment variable");
        }
        else
        {
            // Fall back to file-based credentials (for local development)
            var path = AppDomain.CurrentDomain.BaseDirectory + $"{fireStoreAppOptions.ProjectId}.json";
            
            Console.WriteLine($"Looking for credentials file at: {path}");
            Console.WriteLine($"File exists: {System.IO.File.Exists(path)}");

            if (!System.IO.File.Exists(path))
            {
                throw new FileNotFoundException(
                    $"Firebase credentials not found. " +
                    $"Either set FIREBASE_CREDENTIALS environment variable or " +
                    $"place credentials file at: {path}"
                );
            }

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);
            Console.WriteLine("✅ Firebase credentials loaded from file");
        }

        Console.WriteLine($"Collection.User: {fireStoreAppOptions.Collection.User}");
        Console.WriteLine($"Collection.WorkoutSession: {fireStoreAppOptions.Collection.WorkoutSession}");

        var firestoreDb = FirestoreDb.Create(fireStoreAppOptions.ProjectId);

        Console.WriteLine("✅ Firebase connection created");

        services.AddSingleton(firestoreDb);
        services.AddSingleton(fireStoreAppOptions);
        services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig
        {
            ApiKey = fireStoreAppOptions.ApiKey,
            AuthDomain = $"{fireStoreAppOptions.ProjectId}.firebaseapp.com",
            Providers = new FirebaseAuthProvider[]
            {
                new EmailProvider()
            }
        }));

        services.AddSingleton<IUserService, UserServiceFB>();
        services.AddSingleton<IHashService, HashService>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = $"https://securetoken.google.com/{fireStoreAppOptions.ProjectId}";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{fireStoreAppOptions.ProjectId}",
                    ValidateAudience = true,
                    ValidAudience = fireStoreAppOptions.ProjectId,
                    ValidateLifetime = true
                };
            });

        return services;
    }
}