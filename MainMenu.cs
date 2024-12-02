namespace HelloHoliday;

public class MainMenu
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
        Console.WriteLine("9. Quit");
    }

    public async Task AskUser()
    {
        var response = Console.ReadLine();
        if (response is not null)
        {
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
                case ("9"):
                case ("quit"):
                case ("q"):
                    Console.WriteLine("Quitting");
                    System.Environment.Exit(0);
                    break;
            }
        }
    }
}