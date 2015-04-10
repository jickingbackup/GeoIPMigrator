using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIPDBMigrator
{
    public class GeoIPDBMigrator
    {
        MongoDBController<IPRange> mongoDB = null;

        public GeoIPDBMigrator(string db,string collection, string connection)
        {
            this.mongoDB = new MongoDBController<IPRange>(db, collection, connection, true);
        }

        private void AddIPRange(long startIP,long endIP,string country)
        {
            IPRange log = MongoModelFactory.CreateIPRange();
            log.Start = startIP;
            log.End = endIP;
            log.Country = country;

            mongoDB.Insert(log);
        }

        public bool CheckIfConnected()
        {
            return mongoDB.Connect(); ;
        }

        public List<IPRange> Get()
        {
            return mongoDB.GetAll();
        }

        public string CheckCountryCode(string ip)
        {
            long ipInLong = 0;

            ipInLong = IPHelper.ConvertIPToLong(ip);

            var row = mongoDB.GetAll().SingleOrDefault(i => i.Start <= ipInLong && i.End >= ipInLong);

            if (row != null)
                return row.Country.ToLower();

            return "";
        }

        public long MigrateDataFromCSV(string path, bool convertIPToLong)
        {
            long count = 0;

            using (TextFieldParser parser = new TextFieldParser(path))
            {
                string country = "";
                parser.Delimiters = new string[] { "," };

                while (true)
                {
                    string[] parts = parser.ReadFields();

                    if (parts == null)
                    {
                        break;
                    }

                    if (parts[2].ToString().ToLower() == "nz" && !parts[1].ToString().Contains('f'))
                    {
                        Int64 start = 0;
                        Int64 end = 0;

                        if (convertIPToLong)
                        {
                            start = IPHelper.ConvertIPToLong(parts[0].ToString());
                            end = IPHelper.ConvertIPToLong(parts[1].ToString());
                        }
                        else
                        {
                            Int64.TryParse(parts[0].ToString(), out start);
                            Int64.TryParse(parts[1].ToString(), out end);
                        }

                        country = parts[2].ToString().ToLower();
                        this.AddIPRange(start, end, country);
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
