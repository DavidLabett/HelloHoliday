namespace HelloHoliday;

public class BookingMenu : Menu
{
    Query _query;
    MainMenu _mainMenu;
    Customer _customer;
    CustomerMenu _customerMenu;

    public BookingMenu(Query query, MainMenu mainMenu)
    {
        _query = query;
        _mainMenu = mainMenu;
    }

    public async Task Menu()
    {
        Console.WriteLine("What's your Email?");
        var email = GetInputAsString();

        //handles email input
        var isValid = await _query.ValidateEmail(email);
        if (isValid)
        {
            Console.WriteLine("Email is valid.");
            _customer = await _query.GetCustomer(email);
            PrintBookingMenu();
            await AskUser();
        }
        else
        {
            Console.WriteLine("Email not found.");
            Console.WriteLine("Let's get you registered!");
            await _customerMenu.RegisterCustomer(email);
        }
        
    }

    public void PrintBookingMenu()
    {
        Console.WriteLine("### Booking Menu");
        Console.WriteLine("1. Search available rooms");
        Console.WriteLine("2. Make booking");
        Console.WriteLine("3. Edit booking");
        Console.WriteLine("4. Delete booking");
        Console.WriteLine("0. Return to Main-menu");
    }

    public async Task AskUser()
    {
        var response = GetInputAsString();

        switch (response)
        {
            case "1":
                await SearchRooms();
                break;
            case "2":
                break;
            case "3":
                break;
            case "4":
                break;
            case "0":
                await _mainMenu.Menu();
                break;
        }
    }

    public async Task SearchRooms()
    {
        Console.WriteLine("+------Welcome---to---Booking-------+");
        Console.WriteLine("| When would you like to check in?");
        Console.WriteLine("| (Please use format 'YYYY-MM-DD')");
        DateTime checkIn = GetInputAsDate();

        Console.WriteLine("| When will you check out?");
        DateTime checkOut = GetInputAsDate();

        Console.WriteLine("| What kind of room do you need?");
        Console.WriteLine("| (Single, Double, Triple or Quad?)");
        int roomSize = GetInputAsInt();

        Console.WriteLine("| Do you want a pool? (true/false)");
        bool pool = GetInputAsBool();

        Console.WriteLine("| Should there be entertainment options? (true/false)");
        bool entertainment = GetInputAsBool();

        Console.WriteLine("| Is a kid's club important? (true/false)");
        bool kidsClub = GetInputAsBool();

        Console.WriteLine("| Do you want an on-site restaurant? (true/false)");
        bool restaurant = GetInputAsBool();

        Console.WriteLine("| How far from the beach is okay (in km)");
        int beach = GetInputAsInt();

        Console.WriteLine("| How far from the city-centre is okay (in km)");
        int cityCentre = GetInputAsInt();

        Console.WriteLine("| What matters more to you: ");
        Console.WriteLine("| Great review or great price?");
        string? reviewOrPrice = GetInputAsString();

        //add check that inputs are not null
        await _query.ListBookingPref(new BookingPreferences
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
        });
        await Menu();
    }
}