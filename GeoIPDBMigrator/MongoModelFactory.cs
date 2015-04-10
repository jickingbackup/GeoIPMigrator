using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIPDBMigrator
{
    class MongoModelFactory
    {
        public static IPRange CreateIPRange()
        {
            IPRange log = new IPRange();
            log.Id = ObjectId.GenerateNewId();

            return log;
        }
    }
}
