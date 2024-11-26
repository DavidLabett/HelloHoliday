namespace HelloHoliday;

public class BookingMenu
{
    public void PrintBookingMenu()
    {
        Console.WriteLine("+------Welcome---to---Booking-------+");
        Console.WriteLine("| When would you like to check in?");
        Console.WriteLine("| (Please use format 'YYYY-MM-DD')");
        Console.Write("> ");
        string? checkIn = Console.ReadLine();
        Console.WriteLine("| When will you check out?");
        Console.Write("> ");
        string? checkOut = Console.ReadLine();
        Console.WriteLine("| What kind of room do you need?");
        Console.WriteLine("| (Single, Double, Triple or Quad?)");
        Console.Write("> ");
        string? roomSize = Console.ReadLine().ToLower();
        Console.WriteLine("| Do you want a pool? (Yes/No)");
        Console.Write("> ");
        string? pool = Console.ReadLine().ToLower();
        bool wantsPool = pool == "yes"; //Convert input to bool
        Console.WriteLine("| Should there be entertainment options? (Yes/No)");
        Console.Write("> ");
        string? entertainment = Console.ReadLine().ToLower();
        bool wantsEntertainment = entertainment == "yes"; //Convert input to bool
        Console.WriteLine("| Is a kid's club important? (Yes/No)");
        Console.Write("> ");
        string? kidsClub = Console.ReadLine().ToLower();
        bool wantskidsClub = kidsClub == "yes"; //Convert input to bool
        Console.WriteLine("| Do you want an on-site restaurant? (Yes/No)");
        Console.Write("> ");
        string? restaurant = Console.ReadLine().ToLower();
        bool wantsrestaurant = restaurant == "yes"; //Convert input to bool
        Console.WriteLine("| How far from the beach is okay (in km)");
        Console.Write("> ");
        string? beach = Console.ReadLine().ToLower();
        Console.WriteLine("| How far from the city-centre is okay (in km)");
        Console.Write("> ");
        string? cityCentre = Console.ReadLine().ToLower();
        Console.WriteLine("| What matters more to you: ");
        Console.WriteLine("| Great reviews or great price?");
        Console.Write("> ");
        string? reviewOrPrice = Console.ReadLine().ToLower();
    }
    
}
