namespace PullentiAPI.Controllers
{

    public struct AppConfig
    {
        public required string apiUser { get; set; }
        public required string apiPassword { get; set; }
    }


    public static class ApplicationConfiguration
    {
        private static IConfigurationRoot _configuration;
        static ApplicationConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())

#if DEBUG
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
#else
                .AddEnvironmentVariables()
#endif
                ;
            _configuration = builder.Build();
        }


        public static AppConfig GetConfig()
        {
            AppConfig appCfg = new()
            {
                apiUser = "",
                apiPassword = "",
            };

            appCfg.apiUser = _configuration[key: "apiUser"];
            appCfg.apiPassword = _configuration[key: "apiPassword"];

            return appCfg;
        }
    }

}
