namespace HelloHoliday;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello Holliday!");
        Database database = new();
        // hämta anslutningen (db) att göra queries med
        var db = database.Connection();
        // skapa actions och skicka in anslutningen, så att vi kan köra queries till databasen där
        var query = new Query(db);
        
        
        //TEST HERE:
        var bookingQueries = new BookingsQueriescs(db);
        
            Console.WriteLine("Select an option to test:");
            Console.WriteLine("1. Fetch Available Rooms");
            Console.WriteLine("0. Exit");

            var response = Console.ReadLine();
            switch (response)
            {
                case "1":
                    Console.Write("Enter check-in");
                    var checkIn = Console.ReadLine();
                    var parsedCheckIn = DateTime.Parse(checkIn);
                    Console.Write("Enter check-out");
                    var checkOut = Console.ReadLine();
                    var parsedCheckOut = DateTime.Parse(checkOut);
                    await bookingQueries.FetchAvailableRooms(parsedCheckIn, parsedCheckOut);
                    break;
                case "0":
                    Console.WriteLine("Exit");
                    return;
                default:
                    Console.WriteLine("Invalid option");
                    break;
            }
        }
        //TEST ENDS HERE
        /*
        // skapa en meny och skicka in våra actions i den, så vi kan anropa dem
        MainMenu menu = new(query);
        menu.Menu();
        */
    }
