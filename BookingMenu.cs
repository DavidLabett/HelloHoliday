namespace HelloHoliday;

public class BookingMenu
{
    Query _query;
    MainMenu _mainMenu;
    BookingPreferences _bookingPreferences;

    public BookingMenu(Query query, MainMenu mainMenu)
    {
        _query = query;
        _mainMenu = mainMenu;
    }

    public async Task Menu()
    {
        PrintBookingMenu();
        await AskUser();
    }

    public void PrintBookingMenu()
    {
        Console.WriteLine("### Booking Menu");
        Console.WriteLine("1. Search available rooms");
        Console.WriteLine("2. Make booking");
        Console.WriteLine("3. Edit booking");
        Console.WriteLine("4. Delete booking");
    }

    public async Task AskUser()
    {
        var response = Console.ReadLine();
        if (response is not null)
        {
            switch (response)
            {
                case "1":
                    await SearchRooms();
                    break;
                case "2":
                    await MakeBooking();
                    break;
                case "3":
                    await EditBooking();
                    break;
                case "4":
                    await DeleteBooking();
                    break;
            }
        }
    }

    public async Task SearchRooms()
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

        Console.WriteLine("| Do you want a pool? (true/false)");
        Console.Write("> ");
        string? pool = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Should there be entertainment options? (true/false)");
        Console.Write("> ");
        string? entertainment = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Is a kid's club important? (true/false)");
        Console.Write("> ");
        string? kidsClub = Console.ReadLine()?.ToLower();

        Console.WriteLine("| Do you want an on-site restaurant? (true/false)");
        Console.Write("> ");
        string? restaurant = Console.ReadLine()?.ToLower();

        Console.WriteLine("| How far from the beach is okay (in km)");
        Console.Write("> ");
        string? beach = Console.ReadLine();

        Console.WriteLine("| How far from the city-centre is okay (in km)");
        Console.Write("> ");
        string? cityCentre = Console.ReadLine();

        Console.WriteLine("| What matters more to you: ");
        Console.WriteLine("| Great review or great price?");
        Console.Write("> ");
        string? reviewOrPrice = Console.ReadLine()?.ToLower();

        
        
        //add check that inputs are not null
        _bookingPreferences = new BookingPreferences
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
        await _query.ListBookingPref(_bookingPreferences);
        MakeBooking();
        await Menu();
    }


    public async Task MakeBooking(int customerId)
    {
        Console.WriteLine("Enter room id to make booking");
        int roomId = int.Parse(Console.ReadLine());
        Console.WriteLine("Would you like to add an extra bed for 30$? (true/false)");
        bool extraBed = Boolean.Parse(Console.ReadLine());
        Console.WriteLine("Would you like to include breakfast? (true/false)");
        bool dailyBreakfast = Boolean.Parse(Console.ReadLine());
        await _query.BookRoom(_bookingPreferences, customerId, roomId, extraBed, dailyBreakfast);
        
    }

    public async Task EditBooking()
    {
        
    }

    public async Task DeleteBooking()
    {
        
    }


}


