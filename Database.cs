using Npgsql;
namespace HelloHoliday;

public class Database
{
        // databasuppgifter
        private readonly string _host = "45.10.162.204";
        private readonly string _port = "5437";
        private readonly string _username = "postgres";
        private readonly string _password = "GreedyMotherGrows";
        private readonly string _database = "database_for_menu";

        private NpgsqlDataSource _connection;

        // metod som hämtar anslutningen
        public NpgsqlDataSource Connection()
        {
            return _connection;
        }
    
        // koppla upp till databasen (i constructorn)
        public Database()
        {
            // bygg en anslutningssträng (Adress och inloggning till databasen) 
            string connectionString = $"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database}";
            // använd den för att hämta en anslutning
            _connection = NpgsqlDataSource.Create(connectionString);
        }
    }
