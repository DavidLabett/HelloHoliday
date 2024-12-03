namespace HelloHoliday;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Hello Holiday!");
        
        Database database = new();
        // gets the connection (db) used for making queries (talking to the database)
        var db = database.Connection();
        // creates the query-object where the actions are made and send in the connection so it can talk to the database
        var query = new Query(db);
        
        // creates a menu and sends in our query-object so that the menu can use the querie methods
        MainMenu menu = new(query);
        await menu.Menu();
    }
}