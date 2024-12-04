namespace HelloHoliday;

public class BookingMenu : Menu
{
    Query _query;
    BookingPreferences? _bookingPreferences;
    Customer? _customer;
    CustomerMenu _customerMenu;

    public BookingMenu(Query query, CustomerMenu customerMenu)
    {
        _query = query;
        _customerMenu = customerMenu;
    }

    public async Task Menu()
    {
        PrintBookingMenu();
        await AskUser();
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
        // returns to either BookingMenu or MakeBooking depending on where SearchRooms was called
    }

    public async Task<Customer?> GetCustomer(string email)
    {
        //handles email input
        var isValid = await _query.ValidateEmail(email);
        if (isValid)
        {
            Console.WriteLine("Email is valid.");
            return await _query.GetCustomer(email);
        }
        else
        {
            Console.WriteLine("Email not found.");
            return null;
        }
    }

    public async Task MakeBooking()
    {
        Console.WriteLine("What's your Email?");
        var email = GetInputAsString();
        
        _customer = await GetCustomer(email);
        if (_customer is null)
        {
            await _customerMenu.RegisterCustomer(email);
            _customer = await _query.GetCustomer(email);
        }
        if (_customer is not null)
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

                await _query.BookRoom(_bookingPreferences, _customer.id, roomId, extraBed, dailyBreakfast);

                // confirmation
                Console.WriteLine("+===================================+");
                Console.WriteLine("| Your booking is confirmed!        |");
                Console.WriteLine("| Thank you for choosing us.        |");
                Console.WriteLine("+===================================+");
                Console.WriteLine("[Press any button to continue]");
                Console.ReadLine(); // pause
                // returns to BookingMenu
            }
        }
    }

    public async Task ModifyBooking()
    {
        Console.Clear();
        Console.WriteLine("+===================================+");
        Console.WriteLine("|          MODIFY BOOKING           |");
        Console.WriteLine("+===================================+");
        
        Console.WriteLine("| Please enter your Booking ID:     |");
        Console.WriteLine("+-----------------------------------+");
        int bookingId = GetInputAsInt();

        await _query.FindBooking(bookingId);
        
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
        // MyBooking method here?
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