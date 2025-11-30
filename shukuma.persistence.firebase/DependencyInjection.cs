using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using shukuma.domain.Models;
using shukuma.domain.Interfaces;

namespace shukuma.persistence.firebase;

public static class DependencyInjection
{
    public static IServiceCollection ConnectFirestore(this IServiceCollection services, IConfiguration configuration)
    {
        var fireStoreAppOptions = new FirestoreAppOptions();
        configuration.Bind(nameof(FirestoreAppOptions), fireStoreAppOptions);

        var path = AppDomain.CurrentDomain.BaseDirectory + $"{fireStoreAppOptions.ProjectId}.json";

        // DEBUG: Print the path
        Console.WriteLine($"=== FIREBASE SETUP ===");
        Console.WriteLine($"Looking for credentials at: {path}");
        Console.WriteLine($"File exists: {System.IO.File.Exists(path)}");

        if (!System.IO.File.Exists(path))
        {
            throw new FileNotFoundException($"Firebase credentials file not found at: {path}");
        }

        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

        Console.WriteLine($"Project ID: {fireStoreAppOptions.ProjectId}");
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
