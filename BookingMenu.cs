namespace HelloHoliday;

public class BookingMenu : Menu
{
    Query _query;
    MainMenu _mainMenu;
    BookingPreferences? _bookingPreferences = null;
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
        Console.Clear();
        Console.WriteLine("+===========================+");
        Console.WriteLine("|       BOOKING MENU        |");
        Console.WriteLine("+===========================+");
        Console.WriteLine("| 1. Search Available Rooms |");
        Console.WriteLine("| 2. Make a Booking         |");
        Console.WriteLine("| 3. Edit a Booking         |");
        Console.WriteLine("| 4. Delete a Booking       |");
        Console.WriteLine("| 0. Return to Main Menu    |");
        Console.WriteLine("+===========================+");
        Console.WriteLine("| Select an option:         |");
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
                // await MakeBooking(); // need customer id
                break;
            case "3":
                await EditBooking();
                break;
            case "4":
                await DeleteBooking();
                break;
            case "0":
                await _mainMenu.Menu();
                break;
        }
    }
    

    public async Task SearchRooms()
    {
        Console.Clear();
Console.WriteLine("+===================================+");
Console.WriteLine("|         ROOM BOOKING FORM         |");
Console.WriteLine("+===================================+");
Console.WriteLine("| Please fill in the following info:|");
Console.WriteLine("+-----------------------------------+");

// Check-in date
Console.WriteLine("| When would you like to check in?  |");
Console.WriteLine("| (Format: YYYY-MM-DD)              |");
Console.WriteLine("+-----------------------------------+");
DateTime checkIn = GetInputAsDate();

// Check-out date
Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| When will you check out?          |");
Console.WriteLine("| (Format: YYYY-MM-DD)              |");
Console.WriteLine("+-----------------------------------+");
DateTime checkOut = GetInputAsDate();

// Room type
Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| What type of room do you need?    |");
Console.WriteLine("| (Single, Double, Triple, Quad)    |");
Console.WriteLine("+-----------------------------------+");
int roomSize = GetInputAsInt();

// bools
Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| Do you want a pool? (true/false)  |");
bool pool = GetInputAsBool();

Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| Entertainment options? (true/false)|");
bool entertainment = GetInputAsBool();

Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| Is a kid's club important? (true/false)|");
bool kidsClub = GetInputAsBool();

Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| On-site restaurant? (true/false) |");
bool restaurant = GetInputAsBool();

// Proximity beach and city
Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| Max distance from the beach (km):|");
int beach = GetInputAsInt();

Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| Max distance from city centre (km):|");
int cityCentre = GetInputAsInt();

// Preference review or price
Console.WriteLine("+-----------------------------------+");
Console.WriteLine("| What matters more:                |");
Console.WriteLine("| - Great reviews                   |");
Console.WriteLine("| - Great price                     |");
Console.WriteLine("+-----------------------------------+");
string? reviewOrPrice = GetInputAsString();

// Confirmation message
Console.WriteLine("+===================================+");
Console.WriteLine("| Thank you! Your preferences have  |");
Console.WriteLine("| been recorded. Searching rooms... |");
Console.WriteLine("+===================================+");
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
        await _query.ListAvailableRooms(_bookingPreferences);
        await Menu();
    }


    public async Task MakeBooking(int customerId)
    {
        if (_bookingPreferences is null)
        {
            await SearchRooms();
        }
        else
        {
            Console.Clear();
            Console.WriteLine("+===================================+");
            Console.WriteLine("|        FINALIZE YOUR BOOKING      |");
            Console.WriteLine("+===================================+");

            // Room id input
            Console.WriteLine("| Please enter the Room ID:         |");
            Console.WriteLine("+-----------------------------------+");
            int roomId = GetInputAsInt();

            // Extra_bed option
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| Would you like an extra bed?      |");
            Console.WriteLine("| (Additional $30 per night)        |");
            Console.WriteLine("| Enter true or false:              |");
            Console.WriteLine("+-----------------------------------+");
            bool extraBed = GetInputAsBool();

            // Breakfast option
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| Would you like to include         |");
            Console.WriteLine("| daily breakfast? (true/false)     |");
            Console.WriteLine("+-----------------------------------+");
            bool dailyBreakfast = GetInputAsBool();

            // loading
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| Booking your room, please wait... |");
            Console.WriteLine("+===================================+");

            await _query.BookRoom(_bookingPreferences, customerId, roomId, extraBed, dailyBreakfast);

            // confirmation
            Console.WriteLine("+===================================+");
            Console.WriteLine("| Your booking is confirmed!        |");
            Console.WriteLine("| Thank you for choosing us.        |");
            Console.WriteLine("+===================================+");
            Console.WriteLine("[Press any button to continue]");
            Console.ReadLine(); // pause
        }

    }

    public async Task EditBooking()
    {
        
    }

    public async Task DeleteBooking()
    {
        // My booking metod här?
        Console.Clear();
        Console.WriteLine("+===================================+");
        Console.WriteLine("|         DELETE A BOOKING          |");
        Console.WriteLine("+===================================+");

        // enter booking id
        Console.WriteLine("| Please enter the Booking ID to delete:");
        Console.WriteLine("+-----------------------------------+");
        var bookingId = GetInputAsInt();

        // Confirmation message
        Console.WriteLine("+-----------------------------------+");
        Console.WriteLine("| Deleting your booking, please wait...");
        Console.WriteLine("+-----------------------------------+");

        await _query.DeleteBooking(bookingId);

        // confirmatino message
        Console.WriteLine("+===================================+");
        Console.WriteLine("| Your booking has been deleted     |");
        Console.WriteLine("| successfully.                     |");
        Console.WriteLine("+===================================+");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine();
    }


}