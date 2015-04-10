using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoIPDBMigrator
{
    public class MongoDBController<T> where T : class
    {
        string connStr = string.Empty;
        string dbName = string.Empty;
        string collectionName = string.Empty;
        MongoClient mongodb = null;
        public MongoDatabase database = null;

        public MongoDBController(string dbName, string collectionName, string connection, bool recreateDBOnInitialize)
        {
            this.dbName = dbName;
            this.collectionName = collectionName;
            this.connStr = connection;
            this.mongodb = new MongoClient(this.connStr);
            this.database = this.mongodb.GetServer().GetDatabase(this.dbName);

            if (recreateDBOnInitialize)
            {
                this.DropCollection();
            }
        }

        #region Connection

        public bool Connect()
        {
            this.mongodb.GetServer().Connect();

            if (this.mongodb.GetServer().State == MongoServerState.Connected)
                return true;

            return false;
        }

        public bool Disconnect()
        {
            this.mongodb.GetServer().Disconnect();

            if (this.mongodb.GetServer().State == MongoServerState.Disconnected)
                return true;

            return false;
        }

        #endregion

        #region Collection Creation

        public bool CreateCollection()
        {
            //check if collection exists
            if (this.database.CollectionExists(collectionName))
                return true;

            CommandResult result = this.database.CreateCollection(collectionName);
            if (result.Ok)
                return true;

            return false;
        }

        public void DropCollection()
        {
            if (this.database.CollectionExists(this.collectionName))
                this.database.GetCollection(this.collectionName).Drop();
        }

        #endregion

        #region CRUD
        public List<T> GetAll()
        {
            List<T> result = new List<T>();

            this.Connect();

            MongoCollection<T> collection = GetCollection();

            foreach (var doc in collection.FindAllAs<T>())
            {
                result.Add(doc);
            }

            this.Disconnect();

            return result;
        }

        public void Insert(T entity)
        {
            this.Connect();

            MongoCollection<T> collection = GetCollection();

            collection.Insert(entity);

            this.Disconnect();
        }

        public void Update(T entity)
        {
            this.Connect();

            MongoCollection<T> collection = GetCollection();

            collection.Save<T>(entity);

            this.Disconnect();
        }

        private MongoCollection<T> GetCollection()
        {
            if (!this.database.CollectionExists(this.collectionName))
                this.CreateCollection();

            return this.database.GetCollection<T>(this.collectionName);
        }
        #endregion

    }
}
