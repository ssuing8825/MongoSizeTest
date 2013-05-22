using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Ploeh.AutoFixture;

namespace ConsoleApplication1
{
    class Program
    {

        static void Main(string[] args)
        {
            var v = CreateInitialDocument();

            var collection = GetMongoCollection("Vehicles");
            collection.Insert(v);


            //v = UpdateVehicle(v);
            //collection.Save(v);


            int i = 0;
            do
            {
                v = UpdateVehicle(v);
                collection.Save(v);
                i++;
                Console.WriteLine(i);
            } while (true);

        }

        private static Vehicle UpdateVehicle(Vehicle vehicle)
        {
            var historyVehicle = vehicle.DeepClone();
            //VehicleSnapshot snap = historyVehicle as VehicleSnapshot;

            Mapper.CreateMap<Vehicle, VehicleSnapshot>();
            var snap = Mapper.Map<VehicleSnapshot>(vehicle);

            vehicle.History.Add(snap);
            vehicle.Make = Guid.NewGuid().ToString();
            return vehicle;
        }

        private static Vehicle CreateInitialDocument()
        {
            var fixture = new Fixture();
            var v = fixture.Build<Vehicle>()
                           .Without(c => c.id)
                           .Create();

            return v;

        }


        private static MongoCollection<Vehicle> GetMongoCollection(string collectionName)
        {
            var connectionString = System.Configuration.ConfigurationManager.AppSettings["MongoDBConnectionString"];
            //Still Using ST7
            var databaseName = System.Configuration.ConfigurationManager.AppSettings["MongoDBDatabase"];

            var mongoClient = new MongoClient(connectionString);
            var mongoServer = mongoClient.GetServer();
            var mongoDatabase = mongoServer.GetDatabase(databaseName);
            var collection = mongoDatabase.GetCollection<Vehicle>(collectionName);
            return collection;
        }

    }


    [Serializable]
    public class Vehicle : VehicleSnapshot
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        public IList<VehicleSnapshot> History { get; set; }
    }
    [Serializable]
    public class VehicleSnapshot
    {
        public string MakeYear { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Vin { get; set; }
        public string PropertyId { get; set; }
        public int? VehicleSequenceNumber { get; set; }
        public string DrivenForCode { get; set; }
        public int? DaysDrivenToWork { get; set; }
        public int? DaysDrivenToSchool { get; set; }
        public int? MilesDrivenToWork { get; set; }
        public int? MilesDrivenToSchool { get; set; }
        public int? EstimatedAnnualMileage { get; set; }

        public string MakeYear1 { get; set; }
        public string Make1 { get; set; }
        public string Model1 { get; set; }
        public string Vin1 { get; set; }

        public string MakeYear2 { get; set; }
        public string Make2 { get; set; }
        public string Model2 { get; set; }
        public string Vin2 { get; set; }

        public string MakeYea4r { get; set; }
        public string Make4 { get; set; }
        public string Model4 { get; set; }
        public string Vin4 { get; set; }

        public string MakeYear3 { get; set; }
        public string Make3 { get; set; }
        public string Model3 { get; set; }
        public string Vin3 { get; set; }

        public string MakeYear5 { get; set; }
        public string Make5 { get; set; }
        public string Model5 { get; set; }
        public string Vin5 { get; set; }

        public string MakeYear6 { get; set; }
        public string Make6 { get; set; }
        public string Model6 { get; set; }
        public string Vin6 { get; set; }
    }

    public static class ExtensionMethods
    {
        // Deep clone
        public static T DeepClone<T>(this T a)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }
    }

}
