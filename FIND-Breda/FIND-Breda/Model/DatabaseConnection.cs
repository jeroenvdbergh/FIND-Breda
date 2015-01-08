using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using System.Diagnostics;
using System.IO;
using Windows.Storage;

namespace FIND_Breda.Model
{
    public class DatabaseConnection
    {

        public static string path = Path.Combine(Path.Combine(ApplicationData.Current.LocalFolder.Path, "database"));
        private SQLiteConnection connection;
        private List<Sight> retrievedSights;

        private static DatabaseConnection _databaseConnection = null;
        private static readonly object _padlock = new object();

        public DatabaseConnection()
        {
            connection = new SQLiteConnection(path);

            connection.CreateTable<Sight>();

            retrievedSights = connection.Table<Sight>().ToList<Sight>();
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

        public List<Sight> getSightings()
        {
            return this.retrievedSights;
        }
    }
}