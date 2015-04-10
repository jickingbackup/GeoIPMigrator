using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIPDBMigrator
{
    public class IPRange
    {
        [BsonElementAttribute("_id")]
        public ObjectId Id { get; set; }
        [BsonElementAttribute("start")]
        public Int64 Start { get; set; }
        [BsonElementAttribute("end")]
        public Int64 End { get; set; }
        [BsonElementAttribute("country")]
        public String Country { get; set; }
    }
}
