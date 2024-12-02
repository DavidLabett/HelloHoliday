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
        Console.WriteLine("Choose option");
        Console.WriteLine("1. Customer");
        Console.WriteLine("2. Booking");
        Console.WriteLine("0. Quit");
    }

    public async Task AskUser()
    {
        while (true)
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
                    System.Environment.Exit(0);
                    break;
            }
            PrintMenu();
        }
    }
}