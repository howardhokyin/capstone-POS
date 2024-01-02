using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_System.Database
{
    public class DatabaseHelper : IDisposable
    {
        private MySqlConnection sqlConn;

        public DatabaseHelper(string server, string database, string uid, string password)
        {
            string connectionString = $"Server={server};Database={database};User ID={uid};Password={password};";
            sqlConn = new MySqlConnection(connectionString);
        }

        public bool OpenConnection()
        {
            try
            {
                if (sqlConn.State == System.Data.ConnectionState.Closed)
                {
                    sqlConn.Open();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }

        public void CloseConnection()
        {
            if (sqlConn.State == System.Data.ConnectionState.Open)
            {
                sqlConn.Close();
            }
        }

        public bool AuthenticateUser(string enteredUserId, string enteredPassword)
        {
            if (OpenConnection())
            {
                try
                {
                    // Query to check if the entered user ID and password match a user in the database
                    string query = "SELECT * FROM user WHERE user_id = @userid AND user_password = @password";
                    MySqlCommand command = new MySqlCommand(query, sqlConn);
                    command.Parameters.AddWithValue("@userid", enteredUserId); // Corrected parameter name
                    command.Parameters.AddWithValue("@password", enteredPassword);
                    
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return true; // User is authenticated
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    CloseConnection();
                }
            }

            return false; // Authentication failed or an error occurred
        }

        public string GetUsername(string enteredUserId)
        {
            string username = null;

            if (OpenConnection())
            {
                try
                {
                    string query = "SELECT user_name FROM user WHERE user_id = @userid";
                    MySqlCommand command = new MySqlCommand(query, sqlConn);
                    command.Parameters.AddWithValue("@userid", enteredUserId);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            username = reader["user_name"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                finally
                {
                    CloseConnection();
                }
            }

            return username;
        }

        public void Dispose()
        {
            CloseConnection();
            sqlConn.Dispose();
        }
    }
}
