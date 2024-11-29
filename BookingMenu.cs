namespace HelloHoliday;
public class BookingMenu
{
    //Making sure BookingPreferences are returned in method
    public BookingPreferences Menu()
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
        string? roomSize = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Do you want a pool? (Yes/No)");
        Console.Write("> ");
        string? pool = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Should there be entertainment options? (Yes/No)");
        Console.Write("> ");
        string? entertainment = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Is a kid's club important? (Yes/No)");
        Console.Write("> ");
        string? kidsClub = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Do you want an on-site restaurant? (Yes/No)");
        Console.Write("> ");
        string? restaurant = Console.ReadLine()?.ToLower();

        Console.WriteLine("| How far from the beach is okay (in km)");
        Console.Write("> ");
        string? beach = Console.ReadLine();

        Console.WriteLine("| How far from the city-centre is okay (in km)");
        Console.Write("> ");
        string? cityCentre = Console.ReadLine();

        Console.WriteLine("| What matters more to you: ");
        Console.WriteLine("| Great reviews or great price?");
        Console.Write("> ");
        string? reviewOrPrice = Console.ReadLine()?.ToLower();
        
        //add check that inputs are not null
        return new BookingPreferences
        {
            CheckInDate = checkIn,
            CheckOutDate = checkOut,
            RoomSize = roomSize,
            Pool = pool,
            Entertainment = entertainment,
            KidsClub = kidsClub,
            Restaurant = restaurant,
            DistanceToBeach = beach,
            DistanceToCityCentre = cityCentre,
            Preference = reviewOrPrice
        };
    }
}