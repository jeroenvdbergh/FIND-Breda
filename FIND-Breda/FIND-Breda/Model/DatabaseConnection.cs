using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;
using System.Diagnostics;

namespace FIND_Breda.Model
{
    public class DatabaseConnection
    {
        private SQLiteConnection _connection;
        private List<string> waypoints = new List<string>();

        private static DatabaseConnection _databaseConnection = null;
        private static readonly object _padlock = new object();

        public DatabaseConnection()
        {
            createDatabase();
            insertRecords();
            saveRecords();

            _databaseConnection = this;
        }

        public static DatabaseConnection instance
        {
            get
            {
                lock (_padlock)
                {
                    if (_databaseConnection == null)
                    {
                        _databaseConnection = new DatabaseConnection();
                    }
                    return _databaseConnection;
                }
            }
        }

        /* Parameters: int row, moet liggen tussen de 1-45
         * Returned een string van de database met de informatie van die row
         * Formaat: ID@NAAM@LON@LAT@OPMERKING@ADRES
         */
        public string getRecord(int row)
        {
            string query = @"SELECT * FROM Sight WHERE Id =" + row.ToString();
            string result = String.Empty;
            using (var statement = _connection.Prepare(query))
            {
                statement.Step();
                result = statement[0].ToString() + "@" + statement[1].ToString() + "@" + statement[2].ToString() + "@" + statement[3].ToString() + "@" + statement[4].ToString() + "@" + statement[5].ToString();
            }
            return result;
        }

        /* Geeft een List van string terug met alle waypoints */
        public List<string> getWaypoints()
        {
            return waypoints;
        }

        /*
         * Haalt alle rijen informatie van de database op en slaat ze op in een List van string
         */
        private void saveRecords()
        {
            string query = @"SELECT COUNT(*) FROM Sight";
            string result = String.Empty;
            using (var statement = _connection.Prepare(query))
            {
                statement.Step();
                result = statement[0].ToString();
            }
            int rows = Int32.Parse(result);
            int x = 1;
            while (x <= rows)
            {
                waypoints.Add(getRecord(x));
                x++;
            }
        }

        #region Database initializeren
        private void createDatabase()
        {
            _connection = new SQLiteConnection("database.db");

            string query = @"CREATE TABLE IF NOT EXISTS
                                Sight      (Id           INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
                                            Name         VARCHAR(255),
                                            Longitude    DECIMAL(5),
                                            Latitude     DECIMAL(5),
                                            Description  VARCHAR(255),
                                            Address      VARCHAR(255)
                            );";

            using (var statement = _connection.Prepare(query))
            {
                statement.Step();
            }
        }

        private void insertRecord(int ID, string name, double longitude, double latitude, string description, string address)
        {
            try
            {
                using (var record = _connection.Prepare("INSERT INTO Sight (Id, Name, Longitude, Latitude, Description, Address) VALUES (?,?,?,?,?,?)"))
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


        private void insertRecords()
        {
            //insertRecord(1, "VVV", 51.356467, 4.467650, "Hier is het VVV gevestigd", "Willemstraat");
            // insertRecord(2, "Kasteel van Breda", 51.354367, 4.465700, "Hier is het Kasteel van Breda gevestigd", "Kasteelplein");
            //  insertRecord(3, "VVV", 51.356467, 4.467650, "Hier is het VVV gevestigd", "Willemstraat");
            //  insertRecord(4, "Kasteel van Breda", 51.354367, 4.465700, "Hier is het Kasteel van Breda gevestigd", "Kasteelplein");
            insertRecord(1, "VVV", 51.356467, 4.467650, "Beginpunt", "Willemstraat 17-19");
            insertRecord(2, "Liefdeszuster", 51.355967, 4.467633, "OPMERKING", "Willemstraat");
            insertRecord(3, "Nassau Baronie Monument", 51.355500, 4.467817, "OPMERKING", "Valkenberg");
            insertRecord(4, "Pad ten westen van monument", 51.355500, 4.467633, "OPMERKING", "Valkenberg");
            insertRecord(5, "The Light House", 51.355700, 4.467083, "OPMERKING", "Valkenberg");
            insertRecord(6, "!!!", 51.355600, 4.466750, "OPMERKING", "Valkenberg");
            insertRecord(7, "Einde park", 51.354367, 4.466200, "OPMERKING", "Kasteelplein");
            insertRecord(8, "Kasteel van Breda", 51.354367, 4.465700, "Kasteelplein", "Kasteelplein");
            insertRecord(9, "Stadhouderspoort", 51.353817, 4.465683, "OPMERKING", "Kasteelplein");
            insertRecord(10, "Kruising Kasteelplein/Cingelstraat", 51.354200, 4.465600, "OPMERKING", "Kruising Kasteelplein/Cingelstraat");
            insertRecord(11, "Bocht Cingelstraat", 51.354233, 4.465000, "OPMERKING", "Bocht Cingelstraat");
            insertRecord(12, "Huis van Brecht (rechter zijde)", 51.354017, 4.464617, "OPMERKING", "Cingelstraat 2B");
            insertRecord(13, "Spanjaardsgat (rechter zijde)", 51.354117, 4.464067, "OPMERKING", "Spanjaardsgat");
            insertRecord(14, "Begin Vismarkt", 51.353900, 4.464000, "OPMERKING", "Begin Vismarktstraat");
            insertRecord(15, "Begin Havermarkt", 51.353617, 4.464667, "OPMERKING", "Begin Havermarkt");
            insertRecord(16, "Kruising Torenstraat/Kerkplein", 51.353267, 4.464933, "OPMERKING", "Kruising Torenstraat/Kerkplein");
            insertRecord(17, "Grote Kerk", 51.353300, 4.465167, "OPMERKING", "Kerkplein");
            insertRecord(18, "Kruising Torenstraat/Kerkplein", 51.353267, 4.464933, "OPMERKING", "Kruising Torenstraat/Kerkplein");
            insertRecord(19, "Het Poortje", 51.352917, 4.465083, "OPMERKING", "Lange Brugstraat 5-7");
            insertRecord(20, "Ridderstraat", 51.352250, 4.465450, "OPMERKING", "Ridderstraat");
            insertRecord(21, "Grote Markt", 51.352450, 4.465933, "Zuidpunt Grote Markt", "Ridderstraat 7");
            insertRecord(22, "Bevrijdingsmonument", 51.352817, 4.465800, "OPMERKING", "Grote Markt");
            insertRecord(23, "Stadhuis", 51.353250, 4.465667, "OPMERKING", "Grote Markt");
            insertRecord(24, "Terug naar begin Grote Markt", 51.352783, 4.465817, "OPMERKING", "Grote Markt");
            insertRecord(25, "Zuidpunt Grote Markt", 51.352500, 4.465933, "OPMERKING", "Zuidpunt Grote Markt");
            insertRecord(26, "Antonius van Paduakerk", 51.352583, 4.466350, "OPMERKING", "Sint Janstraat 8");
            insertRecord(27, "Kruising StJanstraat/Molenstraat", 51.352967, 4.467100, "OPMERKING", "Kruising Sint Janstraat/Molenstraat");
            insertRecord(28, "Bibliotheek", 51.352800, 4.467367, "OPMERKING", "Molenstraat 4-6");
            insertRecord(29, "Kruising Molenstraat/Kloosterplein", 51.352417, 4.468133, "OPMERKING", "Kruising Molenstraat/Kloosterplein");
            insertRecord(30, "Kloosterkazerne", 51.352633, 4.468617, "OPMERKING", "Kloosterplein");
            insertRecord(31, "Chasse theater", 51.352650, 4.469200, "OPMERKING", "Kloosterplein");
            insertRecord(32, "Kruising Kloosterplein/Vlaszak", 51.352650, 4.468750, "OPMERKING", "Kruising Kloosterplein/Vlaszak");
            insertRecord(33, "Binding van Isaac", 51.353167, 4.468533, "OPMERKING", "Vlaszak");
            insertRecord(34, "Kruising Vlaszak/Boschstraat", 51.353700, 4.468267, "OPMERKING", "Kruising Vlaszak/Boschstraat");
            insertRecord(35, "Beyerd", 51.353800, 4.468600, "OPMERKING", "Boschstraat 22");
            insertRecord(36, "Kruising Vlaszak/Boschstraat", 51.353700, 4.468267, "OPMERKING", "Kruising Vlaszak/Boschstraat");
            insertRecord(37, "Gasthuispoort", 51.353733, 4.468000, "OPMERKING", "Kruising Vlaszak/Boschstraat");
            insertRecord(38, "Ingang Veemarktstraat", 51.353650, 4.467917, "OPMERKING", "Ingang Veemarktstraat");
            insertRecord(39, "1e bocht Veemarktstraat", 51.353417, 4.467817, "OPMERKING", "1e Veemarktstraat");
            insertRecord(40, "Kruising Veemarktstraat/StAnnastraat", 51.353133, 4.467000, "OPMERKING", "Kruising Veemarktstraat/St. Annastraat");
            insertRecord(41, "Willem Merkxtuin", 51.353467, 4.466767, "Ingang Willem Merkxtuin", "Ingang Willem Merkxtuin");
            insertRecord(42, "Kruising StAnnastraat/Catharinastraat", 51.353800, 4.466683, "OPMERKING", "Kruising St. Annastraat/Catharinastraat");
            insertRecord(43, "Begijnenhof", 51.353817, 4.467017, "Ingang Begijnenhof", "Ingang Begijnenhof");
            insertRecord(44, "Kruising StAnnastraat/Catharinastraat", 51.353800, 4.466683, "OPMERKING", "Kruising St. Annastraat/Catharinastraat");
            insertRecord(45, "Einde stadswandeling", 51.353700, 4.465750, "Eindpunt", "Kasteelplein");

        }
        #endregion
    }
}
