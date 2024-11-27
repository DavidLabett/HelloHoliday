namespace HelloHoliday;

public class MainMenu
{
    //Query _query;
    BookingMenu _bookingMenu;
    CustomerMenu _customerMenu;
    //BookingMenu _bookingMenu = new();
    
    public MainMenu(Query query)
    {
        //_query = query;
        _customerMenu = new(query, this);
        _bookingMenu = new BookingMenu(); //Initialize
    }

    public void Menu()
    {
        bool running = true;
        while (running)
        {
            PrintMenu();
            running = AskUser().Result;
        }
    }
    
    private void PrintMenu()
    {
        Console.WriteLine("Choose option");
        Console.WriteLine("1. Customer");
        Console.WriteLine("2. Booking");
        Console.WriteLine("9. Quit");
    }
    
    private async Task<bool> AskUser()
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
                 await  _customerMenu.Menu();
                    break;
                case ("2"):
                case ("booking"):
                case ("b"):
                    Console.WriteLine("Bookings Menu");
                    _bookingMenu.Menu();
                    break;
                case ("9"):
                case ("quit"):
                case ("q"):
                    Console.WriteLine("Quitting");
                    return false;
            }
        }

        return true;
    }
}