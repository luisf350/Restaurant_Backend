using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Restaurant.Backend.CommonApi.Utils
{
    [ExcludeFromCodeCoverage]
    public static class StringConnectionUtil
    {
        public static async Task<string>GetStringConnection(IConfiguration configuration)
        {
            //var secret = configuration.GetSection("AppSettings:ConnectionStrings:Secret").Value;

            //if (secret.ToUpper() == "LOCAL")
            //{
            //    var connectionString = configuration.GetConnectionString("DefaultConnection");
            //    return string.IsNullOrEmpty(connectionString)
            //        ? throw new Exception("Missing String Connection")
            //        : connectionString;
            //}

            //var tenantId = configuration.GetSection("AppSettings:ConnectionStrings:TenantId").Value;
            //var clientId = configuration.GetSection("AppSettings:ConnectionStrings:ClientId").Value;
            //var clientSecret = configuration.GetSection("AppSettings:ConnectionStrings:ClientSecret").Value;
            //var vaultUri = new Uri(configuration.GetSection("AppSettings:ConnectionStrings:VaultUri").Value);

            //var client = new SecretClient(vaultUri: vaultUri, credential: new ClientSecretCredential(tenantId, clientId, clientSecret));
            //var connectionStringSecret = (client.GetSecretAsync(secret).Result).Value;

            //return connectionStringSecret.Value ?? throw new Exception("Missing String Connection");

            //TODO: Find the way to work with migrations
            return configuration.GetConnectionString("DefaultConnection");
        }
    }
}
