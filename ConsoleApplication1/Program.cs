﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CustomerAccount.Model;
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
            // RunVehicle();

            // RunCustomer();

            CreateOptionSeven();

        }

        private static void CreateOptionSeven()
        {
            var baseDate = new DateTime(2012,1,1);
            var collection = GetMongoCollection("Customers");
            
            var fixture = new Fixture();
            fixture.Register<DateTime>(() => new DateTime(2001, 12, 12));

            var customerBody = fixture.Build<IndividualCustomer>()
                           .Without(c => c.Id)
                           .Create();
            customerBody.OriginalInceptionDate = baseDate;
            customerBody.CustomerAccount[0].CustomerAccountId = ObjectId.GenerateNewId().ToString();

            customerBody.LastUpdated = baseDate;
            collection.Insert(customerBody);
            

            //Update the customer
            customerBody.FirstName = "Steve";
            customerBody.Id = null;
            customerBody.LastUpdated = baseDate.AddDays(5);
            collection.Insert(customerBody);




        }

        private static void RunCustomer()
        {
            var v = CreateInitialCustomer();
            v.History = new List<IndividualCustomer>();
            var collection = GetMongoCollection("Customers");
            collection.Insert(v);

            //v = UpdateVehicle(v);
            //collection.Save(v);


            //int i = 0;
            //do
            //{
            //    v = UpdateCustomer(v);
            //    collection.Save(v);
            //    i++;
            //    Console.WriteLine(i);
            //} while (true);
        }
        private static void RunVehicle()
        {
            var v = CreateInitialDocument();

            var collection = GetMongoCollection("Vehicles");
            collection.Insert(v);

            int i = 0;
            do
            {
                v = UpdateVehicle(v);
                collection.Save(v);
                i++;
                Console.WriteLine(i);
            } while (true);

        }
        private static CustomerParent UpdateCustomer(CustomerParent customer)
        {
            Mapper.CreateMap<Customer, CustomerParent>();
            var snap = Mapper.Map<CustomerParent>(customer);

            customer.History.Add(snap);
            customer.FirstName = Guid.NewGuid().ToString();
            return customer;
        }
        private static Vehicle UpdateVehicle(Vehicle vehicle)
        {
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
        private static CustomerParent CreateInitialCustomer()
        {
            var fixture = new Fixture();
            fixture.Register<DateTime>(() => new DateTime(2001, 12, 12));


            var v = fixture.Build<CustomerParent>()
                           .Without(c => c.Id)
                           .Without(c => c.Id2)
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
    #region Classes

    [Serializable]
    public class CustomerSevenParent
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string AccountNumber { get; set; }
    }

    public class CustomerSevenParentVersion
    {
        //ID would point to the snapshot of the customer
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        //Id of the parent document.
        public string ParentId { get; set; }

        //Account Number used for queries. 
        public string AccountNumber { get; set; }

        //Reason the version was created
        //This could also be a more complext object that tells the system how it changed versions
        public string Reason { get; set; }

        //Effective date of the chaneg  
        public DateTime EffectiveDate { get; set; }
    }

    [Serializable]
    public class CustomerParent : IndividualCustomer
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id2 { get; set; }
        public IList<IndividualCustomer> History { get; set; }
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

    #endregion EndClasses
}
