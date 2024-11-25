namespace HelloHoliday;

public class Menu
{
    Query _query;
    public Menu(Query query)
    {
        // constructorn tar emot actions 
        _query = query;
        // och startar menyn
        PrintMenu();
    }
    private void PrintMenu()
    {
        // skriver ut menyn i konsolen
        Console.WriteLine("Choose option");
        Console.WriteLine("1. List all");
        Console.WriteLine("2. Show one");
        Console.WriteLine("3. Add one");
        Console.WriteLine("4. Update one");
        Console.WriteLine("5. Delete one");
        Console.WriteLine("9. Quit");
        // lyssnar på användaren
        AskUser();
    }
    private async void AskUser()
    {
        // tar emot vad användaren skriver
        var response = Console.ReadLine();
        if (response is not null)
        {
            string? id; // define for multiple use below
            
            // kör olika actions beroende på vad användaren skrivit
            switch (response)
            {
                case("1"):
                    Console.WriteLine("Listing all");
                    _query.ListAll();
                    break;
                case("2"):
                    Console.WriteLine("Enter id to show details about one");
                    id = Console.ReadLine();
                    if (id is not null)
                    { 
                        _query.ShowOne(id);
                    }
                    break;
                case("3"):
                    Console.WriteLine("Enter name (required)");
                    var name = Console.ReadLine(); // required
                    Console.WriteLine("Enter slogan");
                    var slogan = Console.ReadLine(); // not required
                    if (name is not null)
                    {
                        _query.AddOne(name, slogan);
                    }
                    break;
                case("4"):
                    Console.WriteLine("Enter id to update one");
                    id = Console.ReadLine();
                    if (id is not null)
                    { 
                        _query.UpdateOne(id);
                    }
                    break;
                case("5"):
                    Console.WriteLine("Enter id to delete one");
                    id = Console.ReadLine();
                    if (id is not null)
                    { 
                        _query.DeleteOne(id);
                    }
                    break;
                case("9"):
                    Console.WriteLine("Quitting");
                    Environment.Exit(0);
                    break;
            }

            PrintMenu();
        }
        
    }
    
}
