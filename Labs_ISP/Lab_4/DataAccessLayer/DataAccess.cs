using System.Data.SqlClient;
using Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccessLayer
{
    public class DataAccess
    {

        private readonly string connectionString;
        private readonly SqlConnection sqlConnection;

        public DataAccess(string connectionString = null)
        {
            this.connectionString = connectionString;
            sqlConnection = new SqlConnection(connectionString);
        }

        public List<Person> GetPersons()
        {
            string sqlCommandName = "GetPersonInfo";
            var persons = new List<Person>();

            try
            {
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(sqlCommandName, sqlConnection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader sqlReader = command.ExecuteReader();

                if (sqlReader.HasRows)
                {
                    while (sqlReader.Read())
                    {
                        Person obj = new Person();
                        obj.Id = sqlReader.GetInt32(0);
                        obj.FirstName = sqlReader.GetString(1);
                        obj.LastName = sqlReader.GetString(2);
                        obj.Address = sqlReader.GetString(3);
                        obj.City = sqlReader.GetString(4);
                        obj.PhoneNumber = sqlReader.GetString(5);
                        obj.EmailAddress = sqlReader.GetString(6);
                        persons.Add(obj);
                    }
                }
                sqlReader.Close();
            }
            catch (Exception ex)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "errors.txt");
                using StreamWriter sr = new StreamWriter(path, true);
                sr.Write(ex.Message);
            }
            finally
            {
                sqlConnection.Close();
            }
            return persons;
        }
    }
}
