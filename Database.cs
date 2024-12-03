using Npgsql;
namespace HelloHoliday;

public class Database
{
        // database details
        private readonly string _host = "45.10.162.204";
        private readonly string _port = "5437";
        private readonly string _username = "postgres";
        private readonly string _password = "GreedyMotherGrows";
        private readonly string _database = "hello_holiday";

        private NpgsqlDataSource _connection;
        
        // method for getting the connection
        public NpgsqlDataSource Connection()
        {
            return _connection;
        }
    
        // connects to the database (in the constructor)
        public Database()
        {
            // builds the connection string (address and login for the database)
            string connectionString = $"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database}";
            // used for getting the connection
            _connection = NpgsqlDataSource.Create(connectionString);
        }
    }
