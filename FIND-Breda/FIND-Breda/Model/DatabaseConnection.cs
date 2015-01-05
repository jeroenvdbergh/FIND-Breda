using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace FIND_Breda.Model
{
    class DatabaseConnection
    {

        SQLiteConnection connection;

        public DatabaseConnection()
        {
            createDatabase();
            insertRecords();
        }

        public void createDatabase()
        {
            connection = new SQLiteConnection("database.db");

            string query = @"CREATE TABLE IF NOT EXISTS
                                Sight      (Id           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                            Name         VARCHAR(255),
                                            Longitude    DECIMAL(5),
                                            Latitude     DECIMAL(5),
                                            Description  VARCHAR(255),
                                            Address      VARCHAR(255)
                            );";

            using (var statement = connection.Prepare(query))
            {
                statement.Step();
            }
        }

        public void insertRecord(int ID, string name, double longitude, double latitude, string description, string address)
        {
            try
            {
                using (var record = connection.Prepare("INSERT INTO Sight (Id, Name, Longitude, Latitude, Description, Address) VALUES (?,?,?,?,?,?)"))
                {
                    record.Bind(1, ID);
                    record.Bind(2, name);
                    record.Bind(3, longitude);
                    record.Bind(4, latitude);
                    record.Bind(5, description);
                    record.Bind(6, address);
                    record.Step();
                }


            }
            catch (Exception e)
            {
                //afhandelen
            }
        }

        public void testMethod()
        {
            string query = @"SELECT Name FROM Sight";

            using (var statement = connection.Prepare(query))
            {
                statement.Step();
                string x = statement[0].ToString();
            }
        }

        public void insertRecords()
        {
            insertRecord(1, "VVV", 51.356467, 4.467650, "Hier is het VVV gevestigd", "Willemstraat");
            insertRecord(2, "Kasteel van Breda", 51.354367, 4.465700, "Hier is het Kasteel van Breda gevestigd", "Kasteelplein");
            insertRecord(3, "VVV", 51.356467, 4.467650, "Hier is het VVV gevestigd", "Willemstraat");
            insertRecord(4, "Kasteel van Breda", 51.354367, 4.465700, "Hier is het Kasteel van Breda gevestigd", "Kasteelplein");
        }
    }
}
