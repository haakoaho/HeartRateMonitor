using Heart_Monitor_API;
using Heart_Monitor_API.Constants;
using Heart_Monitor_API.Models;
using Heart_Monitor_API.NewFolder;
using Npgsql;
using NUnit.Framework;
using System;
using System.Collections.Generic;

;

namespace Heart_Monitor_API_Test
{
    class HealthDBIntegrationTest
    {
        CreateHeartRateModel model;
        [SetUp]
        public void Setup()
        {
            new HealthDB(Databases.TestDB).DropDatabase();
            new HealthDB(Databases.TestDB).CreateTablesIfNotExists();
            model = new()
            {
                ArterisPressure = 100,
                SystolicPressure = 100,
                UserId = 1
            };
        }

        [Test]
        public void FindRegisteredUser()
        {
            new HealthDB(Databases.TestDB).RegisterUser("b");
            long userId = new HealthDB(Databases.TestDB).GetUserId("b");
            Assert.AreEqual(1, userId);

        }

        [Test]
        public void DatabaseDropsBetweenTests()
        {
            new HealthDB(Databases.TestDB).RegisterUser("b");
            long userId = new HealthDB(Databases.TestDB).GetUserId("b");
            Assert.AreEqual(1, userId);
        }
        [Test]
        public void UserNotExistShouldThrow()
        {
            Assert.Throws<NullReferenceException>(  //this should be handled better than nullrefence but yes
                () => { new HealthDB(Databases.TestDB).GetUserId("a"); });
        }
        [Test]
        public void UserNotExistShouldThrowForeignKeyException()
        {
            Assert.Throws<PostgresException>(  
                () => { new HealthDB(Databases.TestDB).InsertHeartRateRecord(model); });
        }
        [Test]
        public void InsertHealthRecordSuccess() {
            new HealthDB(Databases.TestDB).RegisterUser("b");
            new HealthDB(Databases.TestDB).InsertHeartRateRecord(model);
            List<HeartRateEntity> entities = new HealthDB(Databases.TestDB).FindHeartRateByName(model.UserId);
            Assert.Equals(1, entities.Count);
            Assert.Equals(model.ArterisPressure, entities[0].ArterisPressure);
            Assert.Equals(model.SystolicPressure, entities[0].SystolicPressure);
        }
        [Test]
        public void DelteRecord()
        {
            new HealthDB(Databases.TestDB).RegisterUser("b");
            new HealthDB(Databases.TestDB).InsertHeartRateRecord(model);
            List<HeartRateEntity> entities = new HealthDB(Databases.TestDB).FindHeartRateByName(model.UserId);
            new HealthDB(Databases.TestDB).DeleteHeartRate(entities[0].RecordId);
            List<HeartRateEntity> entities2 = new HealthDB(Databases.TestDB).FindHeartRateByName(model.UserId);
            Assert.Equals(0, entities2.Count);
        }
    }
}