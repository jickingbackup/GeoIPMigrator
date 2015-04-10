using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIPDBMigrator.ConsoleApp
{
    class Program
    {
        static long totalCount = 0;
        static string csvPaths = ConfigurationManager.AppSettings["csvPaths"] as string;
        static string mongoconnection = ConfigurationManager.AppSettings["mongoconnection"] as string;
        static string mongodb = ConfigurationManager.AppSettings["mongodb"] as string;
        static string mongocollection = ConfigurationManager.AppSettings["mongocollection"] as string;
        //static string dbIPCSVPath = ConfigurationManager.AppSettings["dbIPCSVPath"] as string; //@"\\citfs\Users\JBebiro\ICOMMIT-25757\dbip-country.csv"; //convert to long
        //static string IP2LocationPath = ConfigurationManager.AppSettings["IP2LocationPath"] as string;//@"\\citfs\Users\JBebiro\ICOMMIT-25757\dbip-country.csv";
        //static string ipligenceLite = ConfigurationManager.AppSettings["ipligenceLite"] as string;//@"\\citfs\Users\JBebiro\ICOMMIT-25757\dbip-country.csv";
        static GeoIPDBMigrator migrator = new GeoIPDBMigrator(mongodb, mongocollection, mongoconnection);

        static void Main(string[] args)
        {
            long temp = 0;
            //connect
            if (!migrator.CheckIfConnected())
                return;

            //Connect to mongo
            Console.Title = "GeoIP Migrator";
            Console.WriteLine("connected to {0}", mongoconnection);
            Console.WriteLine("database {0}", mongodb);
            Console.WriteLine("using {0}", mongocollection);
            Console.WriteLine("========================================");
            
            //read/insert data
            string[] paths = csvPaths.Split(',');

            for (int i = 0; i < paths.Length; i++)
            {
                string[] path = paths[i].Split('|');
                bool convertIPToLong = false;

                if (path[1].ToLower() == "true")
                    convertIPToLong = true;

                temp = 0;

                Console.WriteLine("inserting data from {0}", path[0]);
                temp = migrator.MigrateDataFromCSV(path[0], convertIPToLong);
                Console.WriteLine("inserted  {0}", temp);
                totalCount = totalCount + temp;
                Console.WriteLine("========================================");
            }

            //end
            Console.WriteLine("total inserted rows: {0}", totalCount);
            Console.WriteLine("total rows: {0}", migrator.Get().Count);
            //Console.WriteLine("Press any key to exit...");
            //Console.ReadLine();
            while (true)
            {
                TestIP();
            }
        }

        static void TestIP()
        {
            string ip = "";
            Console.WriteLine("Enter ip");
            ip = Console.ReadLine();

            string country = migrator.CheckCountryCode(ip);

            if (country.ToLower() == "nz")
                Console.WriteLine("Country NZ");
            else
                Console.WriteLine("Country Unknown");

            Console.WriteLine("enter any key...");
            Console.WriteLine("===============================");
            Console.ReadLine();
        }
    }
}
