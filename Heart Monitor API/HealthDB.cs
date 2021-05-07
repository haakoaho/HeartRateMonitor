using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Heart_Monitor_API.Constants;
using Heart_Monitor_API.Models;
using Heart_Monitor_API.NewFolder;
using Microsoft.EntityFrameworkCore;
using Npgsql;


namespace Heart_Monitor_API
{
    public class HealthDB
    {
        public bool isProduction;
        private NpgsqlConnection connection;
        public HealthDB(string db) {
            ConnectToDB(db);
            isProduction = db == Databases.ProductionDB;
        }

        private void ConnectToDB(string db)
        {
            NpgsqlConnectionStringBuilder sb = new()
            {
                Host = "localhost",
                Port = 5432,
                Database = db,
                Username = "postgres",
                Password = "SuperPassword",
                Timeout = 20,
                CommandTimeout = 20
            };
            connection = new NpgsqlConnection(sb.ConnectionString);
            connection.Open();
        }

        public long GetUserId(string username)
        {
            using var cmd = new NpgsqlCommand("select user_id from users where username = (@a)", connection);
            cmd.Parameters.AddWithValue("a", username);
            return (long)cmd.ExecuteScalar();
        }

        public void InsertHeartRateRecord(CreateHeartRateModel heartRateModel)
        {
            using var cmd = new NpgsqlCommand("insert into heart_record(owner, systolic_pressure, arteris_pressure) " +
                "values((@a),(@b),(@c))", connection);
            cmd.Parameters.AddWithValue("a",heartRateModel.UserId);
            cmd.Parameters.AddWithValue("b", heartRateModel.SystolicPressure);
            cmd.Parameters.AddWithValue("c", heartRateModel.ArterisPressure);
            cmd.ExecuteNonQuery();
        }


        public void RegisterUser(string username)
        {
            using var cmd = new NpgsqlCommand("insert into users(username) values((@a))", connection);
            cmd.Parameters.AddWithValue("a", username);
            cmd.ExecuteNonQuery();
        }

        public void DeleteHeartRate(long recordId)
        {
            using var cmd = new NpgsqlCommand("delete from heart_record where record_id = (@a)", connection);
            cmd.Parameters.AddWithValue("a", recordId);
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// Creates the tables of the database if they don't exists
        /// </summary>
        public void CreateTablesIfNotExists()
        {
            using var cmd = new NpgsqlCommand(
                "create table if not exists users (user_id bigserial primary key, username text unique);" +
                "create table if not exists heart_record" +
                " (record_id bigserial primary key, owner bigint references users(user_id), systolic_pressure real, arteris_pressure real , created_at timestamp default now());", connection);
            cmd.ExecuteNonQuery();
        }

        public List<HeartRateEntity> FindHeartRateByName(long name)
        {
            List<HeartRateEntity> list = new();
            using var cmd = new NpgsqlCommand("select * from heart_record where owner = (@p) order by created_at desc", connection);
            cmd.Parameters.AddWithValue("p", name);
            NpgsqlDataReader rdr = cmd.ExecuteReader();
            while (rdr.Read())
            {
                HeartRateEntity heartRateModel = new HeartRateEntity();
                heartRateModel.RecordId = rdr.GetInt64(0);
                heartRateModel.UserId = rdr.GetInt64(1);
                heartRateModel.SystolicPressure = rdr.GetFloat(2);
                heartRateModel.ArterisPressure = rdr.GetFloat(3);
                heartRateModel.CreatedAt = rdr.GetDateTime(4);
                list.Add(heartRateModel);
            }
            return list;
        }

        /// <summary>
        /// Drops all the tables in the database
        /// </summary>
        public void DropDatabase()
        {
            if (isProduction)
            {
                throw new ArgumentException("Cannot drop production database");
            }
            using var cmd = new NpgsqlCommand("drop table if exists heart_record; drop table if exists users;", connection);
            cmd.ExecuteNonQuery();
        }
        
    }
}
