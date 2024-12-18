namespace HelloHoliday;

public class BookingMenu : Menu
{
    Query _query;
    BookingPreferences? _bookingPreferences;
    Customer _customer;

    public BookingMenu(Query query)
    {
        _query = query;
    }

    public async Task Menu(Customer customer)
    {
        _customer = customer;
        if (customer is not null)
        {
            PrintBookingMenu();
            await AskUser ();
        }
        else
        {
            Console.WriteLine("*Please login before you can access this feature*");
            Console.ReadLine();
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
        Console.WriteLine("| 3. Modify a Booking       |");
        Console.WriteLine("| 4. Delete a Booking       |");
        Console.WriteLine("| 0. Return to Main Menu    |");
        Console.WriteLine("+===========================+");
        Console.WriteLine("| Select an option:         |");
    }

    public async Task AskUser()
    {
        bool continueMenu = true;
        while (continueMenu) // so we get a valid answer before continuing
        {
            var response = GetInputAsString();

            switch (response)
            {
                case "1":
                    await SearchRooms();
                    break;
                case "2":
                    await MakeBooking();
                    break;
                case "3":
                    await ModifyBooking();
                    break;
                case "4":
                    await DeleteBooking();
                    break;
                case "0":
                    continueMenu = false;
                    break;
            }
            if (continueMenu) // if a wrong input is given print the menu and try again
            {
                PrintBookingMenu();
            }
        }
        _bookingPreferences = null; // nulls preferences when leaving bookingmenu
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
        int roomSize = GetInputAsRoomSize();

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
        string reviewOrPrice = GetInputAsString();

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
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine(); // pause
        // returns to either BookingMenu or MakeBooking depending on where SearchRooms was called
    }

   public async Task MakeBooking()
{
    if (_bookingPreferences is null)
    {
        await SearchRooms();
    }
    else
    {
        // The rest of your booking logic remains unchanged
        Console.Clear();
        Console.WriteLine("+===================================+");
        Console.WriteLine("|        FINALIZE YOUR BOOKING      |");
        Console.WriteLine("+===================================+");
        //Print Available Rooms based on preferences
        await _query.ListAvailableRooms(_bookingPreferences);
        // Room id input
        Console.WriteLine("| Please enter the Room ID:         |");
        Console.WriteLine("+-----------------------------------+");
        int roomId = GetInputAsInt();

        // Extra_bed option
        Console.WriteLine("+-----------------------------------+");
        Console.WriteLine("| Would you like an extra bed?      |");
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

        await _query.BookRoom(_bookingPreferences, _customer.id, roomId, extraBed, dailyBreakfast);

        // confirmation
        Console.WriteLine("+===================================+");
        Console.WriteLine("| Your booking is confirmed!        |");
        Console.WriteLine("| Thank you for choosing us.        |");
        Console.WriteLine("+===================================+");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine(); // pause
    }
}

    public async Task ModifyBooking()
    {
        Console.Clear();
        await _query.MyBookings(_customer.id); //prints the customers current bookings
        Console.WriteLine("+===================================+");
        Console.WriteLine("|          MODIFY BOOKING           |");
        Console.WriteLine("+===================================+");
        
        Console.WriteLine("| Please enter your Booking ID:     |");
        Console.WriteLine("+-----------------------------------+");
        int bookingId = GetInputAsInt();

        await _query.FindBooking(bookingId);
        
        // Extra_bed option
        Console.WriteLine("+-----------------------------------+");
        Console.WriteLine("| Toggle extra bed                  |");
        Console.WriteLine("| Enter true or false:              |");
        Console.WriteLine("+-----------------------------------+");
        bool extraBed = GetInputAsBool();

        // Breakfast option
        Console.WriteLine("+-----------------------------------+");
        Console.WriteLine("| Toggle daily breakfast            |");
        Console.WriteLine("| (true/false)                      |");
        Console.WriteLine("+-----------------------------------+");
        bool dailyBreakfast = GetInputAsBool();
        
        await _query.ModifyBooking(bookingId, extraBed, dailyBreakfast);
        
        // confirmation
        Console.WriteLine("+===================================+");
        Console.WriteLine("| Your booking has been modified!   |");
        Console.WriteLine("+===================================+");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine(); // pause
        // returns to bookingMenu
    }

    public async Task DeleteBooking()
    {
        Console.Clear();
        await _query.MyBookings(_customer.id);
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

        // confirmation message
        Console.WriteLine("+===================================+");
        Console.WriteLine("| Your booking has been deleted     |");
        Console.WriteLine("| successfully.                     |");
        Console.WriteLine("+===================================+");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine();
        // returns to BookingMenu
    }
}