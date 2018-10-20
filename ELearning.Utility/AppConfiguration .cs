using System;
using Microsoft.Extensions.Configuration;

namespace ELearning.Utility
{

    public class AppConfiguration
    {
        private IConfigurationRoot Configuration { get; }
        IConfiguration appSetting;
       
        public AppConfiguration(IConfiguration config)
        {
            appSetting = config;
        }

        public string connectionString
        {
            get => appSetting["ApplicationSettings:ConnectionString"];
        }

        public string adminEmailAddress
        {
            get => appSetting["ApplicationSettings:AdminEmailAddress"];
        }

        public string storageKey
        {
            get => appSetting["ApplicationSettings:storageKey"];
        }

        public string containerName
        {
            get => appSetting["ApplicationSettings:containerName"];
        }

        public string iotHubConnectionString
        {
            get => appSetting["ApplicationSettings:iotHubConnectionString"];
        }

        public string iotHubStorage
        {
            get => appSetting["ApplicationSettings:iotHubStorage"];
        }

        public string iotHubHostName
        {
            get => appSetting["ApplicationSettings:iotHubHostName"];
        }

        public string smtpUserName
        {
            get => appSetting["ApplicationSettings:smtpUserName"];
        }

        public string smtpPassword
        {
            get => appSetting["ApplicationSettings:SmtpPassword"];
        }

        public string SMTPServer
        {
            get => appSetting["ApplicationSettings:SMTPServer"];
        }

        public int SMTPPort
        {
            get => Convert.ToInt32(appSetting["ApplicationSettings:SMTPPort"]);
        }

        public string FromEmailAddress
        {
            get => appSetting["ApplicationSettings:FromEmailAddress"];
        }
    }
}
