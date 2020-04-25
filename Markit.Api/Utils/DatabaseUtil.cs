using System;
using System.Collections.Generic;
using System.Linq;
using Markit.Api.Interfaces.Utils;

namespace Markit.Api.Utils
{
    public class DatabaseUtil : IDatabaseUtil
    {
        private string _connectionString;
        
        public DatabaseUtil()
        {
            BuildConnectionString();
        }

        public string GetConnectionString()
        {
            return _connectionString;
        }

        private void BuildConnectionString()
        {
            var azureStrings = Environment.GetEnvironmentVariable("MYSQLCONNSTR_localdb").Split(";");
            var azureStringsDictionary = new Dictionary<string, string>();
            foreach (var str in azureStrings)
            {
                var subStr = str.Split("=");
                azureStringsDictionary[subStr.ElementAt(0)] = subStr.ElementAt(1);
            };

            var dataSource = azureStringsDictionary["Data Source"].Split(":");
            _connectionString = $"Database=markitapi;" +
                                $"Server={dataSource.ElementAt(0)};" +
                                $"Port={dataSource.ElementAt(1)};" +
                                $"Uid={azureStringsDictionary["User Id"]};" +
                                $"Password={azureStringsDictionary["Password"]};" +
                                "Allow User Variables=true";
        }
    }
}