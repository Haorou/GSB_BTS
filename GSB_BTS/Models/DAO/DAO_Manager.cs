using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using System.Configuration;

namespace GSB.Models.DAO
{
    public class DAO_Manager
    {
        protected MySqlConnection manager;
        protected MySqlCommand command;
        protected MySqlDataReader dataReader;
        public DAO_Manager()
        {
            if(manager == null)
            {
                string host = "localhost";
                int port = 3308;
                string database = "gsb";
                string username = "root";
                string password = "";

                String connectionString = "SERVER=" + host + ";PORT=" + port + ";DATABASE=" +
                                            database + ";UID=" + username + ";PASSWORD=" + password + ";";

                manager = new MySqlConnection(connectionString);
            }
        }

        public bool OpenConnection()
        {
            bool isOpen = false;
            try
            {
                manager.Open();
                isOpen = true;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e);
            }
            return isOpen;
        }

        public bool CloseConnection()
        {
            bool isClose = false;
            try
            {
                manager.Close();
                isClose = true;
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e);
            }
            return isClose;
        }

    }
}
