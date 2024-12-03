namespace HelloHoliday;

public class MainMenu : Menu
{
    BookingMenu _bookingMenu;
    CustomerMenu _customerMenu;

    public MainMenu(Query query)
    {
        _customerMenu = new(query, this);
        _bookingMenu = new BookingMenu(query, this);
    }

    public async Task Menu()
    {
        PrintMenu();
        await AskUser();
    }

    public void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("+=========================+");
        Console.WriteLine("|       MAIN MENU         |");
        Console.WriteLine("+=========================+");
        Console.WriteLine("| 1. Customer Menu        |");
        Console.WriteLine("| 2. Booking Menu         |");
        Console.WriteLine("| 0. Quit                 |");
        Console.WriteLine("+=========================+");
        Console.WriteLine("| Select an option:       |");
    }

    public async Task AskUser()
    {
        bool continueMenu = true;
        while (continueMenu) // this method sends the user to other functions or it exits the program
        // by having this loop we make sure that the program doesn't end until we tell it to
        {
            var response = GetInputAsString();

            switch (response)
            {
                case ("1"):
                case ("customer"):
                case ("c"):
                    Console.WriteLine("Customer Menu");
                    await _customerMenu.Menu();
                    break;
                case ("2"):
                case ("booking"):
                case ("b"):
                    Console.WriteLine("Bookings Menu");
                    await _bookingMenu.Menu();
                    break;
                case ("0"):
                case ("quit"):
                case ("q"):
                    Console.WriteLine("Quitting");
                    continueMenu = false;
                    break;
            }
            if (continueMenu)
            {
                PrintMenu();
            }
        }
    }
}