using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceDB.Models
{
    public class ApplicationSettings
    {
        private readonly IConfiguration _configuration;

        public ApplicationSettings(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string ConnectionString => _configuration.GetValue<string>(nameof(ConnectionString));
        public string LoadBotJobCron => _configuration.GetValue<string>(nameof(LoadBotJobCron));
    }
}
