using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Collections.Concurrent;
using System.Reflection;

namespace datntdev.Microservices.Common.Configuration
{
    public static class AppConfiguration
    {
        private static readonly ConcurrentDictionary<string, IConfigurationRoot> _configurationCache = new();

        public static IConfigurationRoot Get(IHostEnvironment env)
        {
            var path = env.ContentRootPath;
            var envName = env.EnvironmentName;
            var useSecret = env.IsDevelopment();
            var cacheKey = path + "#" + envName + "#" + useSecret;
            return _configurationCache.GetOrAdd(cacheKey, _ => BuildConfiguration(path, envName, useSecret));
        }

        private static IConfigurationRoot BuildConfiguration(string path, string envName, bool useSecret = false)
        {
            var builder = new ConfigurationBuilder().SetBasePath(path).AddJsonFile("appsettings.json");

            if (!string.IsNullOrWhiteSpace(envName))
            {
                builder = builder.AddJsonFile($"appsettings.{envName}.json", optional: true);
            }

            builder = builder.AddEnvironmentVariables();

            if (useSecret)
            {
                builder.AddUserSecrets(Assembly.GetEntryAssembly() ?? throw new NullReferenceException(), true);
            }

            return builder.Build();
        }
    }
}
