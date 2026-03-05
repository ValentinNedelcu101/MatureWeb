using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Northwind.Mvc.Data;
using Northwind.EntityModels;
using Microsoft.Data.SqlClient;
namespace Northwind.Mvc.Extensions;

    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddIdentityDatabase(this WebApplicationBuilder builder)
    {
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddEntityFrameworkStores<ApplicationDbContext>();
        return builder;
    }

    public static WebApplicationBuilder AddNorthwindDatabase(this WebApplicationBuilder builder)
    {
        string? sqlServerConnection = builder.Configuration
    .GetConnectionString("NorthwindConnection");

        if (sqlServerConnection is null)
        {
            WriteLine("Northwind database connection string is missing from configuration");
        }
        else
        {
            SqlConnectionStringBuilder sql = new(sqlServerConnection);

            sql.IntegratedSecurity = false;
            sql.UserID = Environment.GetEnvironmentVariable("MY_SQL_USR");
            sql.Password = Environment.GetEnvironmentVariable("MY_SQL_PWD");

            builder.Services.AddNorthwindContext(sql.ConnectionString);
        }
        return builder;
    }

    }

