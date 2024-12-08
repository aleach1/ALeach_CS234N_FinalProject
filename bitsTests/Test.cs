using bitsEFClasses;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bitsTests
{ 

    [TestFixture]
    public class Test
    {
        [Test]
        public void ConnectionStringTest()
        {
            string folder = System.AppContext.BaseDirectory;
            var builder = new ConfigurationBuilder()
                    .SetBasePath(folder)
                    .AddJsonFile("mySqlSettings.json", optional: true, reloadOnChange: true);

            string connectionString = builder.Build().GetConnectionString("mySql");
            Console.WriteLine(connectionString);
            Assert.NotNull(connectionString);
        }
    }
}
