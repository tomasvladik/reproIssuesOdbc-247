using System;
using NUnit.Framework;
using OdbcTest;

namespace DatabaseWorkerTests
{
    [TestFixture]
    public class MyQueryRunnerTests
    {
        private const string Query = @"
SELECT 1;
SELECT 1;
";



        [Test]
        public void Execute_AgainstRealDb()
        {
            
            string dbServerAddress="127.0.0.1"; //change to match your environment
            string dbRootPassword="Supercalifragilisticexpialidocious"; //change to match your environment


            var executor = new MyQueryRunner();
            executor.ExecuteQuery(
                $"Driver={{MariaDB ODBC 3.1 Driver}};Server={dbServerAddress};User=root;Password={dbRootPassword};Option=3;Port=3306",
                Query
                , TimeSpan.FromSeconds(30));
        }

    }
}
