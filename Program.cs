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
        
        // skapa en meny och skicka in våra actions i den, så vi kan anropa dem
        MainMenu menu = new(query);
        await menu.Menu();
    }
    
    
}