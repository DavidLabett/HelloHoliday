namespace HelloHoliday;

public class MainMenu : Menu
{
    BookingMenu _bookingMenu;
    CustomerMenu _customerMenu;
    Query _query;
    static Customer? _currentCustomer;
    
   public MainMenu(Query query)
    {
        _query = query; // Initialize the Query object
        _customerMenu = new CustomerMenu(query);
        _bookingMenu = new BookingMenu(query);
    }

    public async Task Menu()
    {
        PrintMenu();
        await AskUser ();
    }

    public void PrintMenu()
    {
        Console.Clear();
        Console.WriteLine("+=========================+");
        Console.WriteLine("|       MAIN MENU         |");
        Console.WriteLine("+=========================+");
        Console.WriteLine("| 1. Customer Menu        |");
        Console.WriteLine("| 2. Booking Menu         |");
        Console.WriteLine("| 3. Login Customer       |");
        Console.WriteLine("| 4. Logout               |"); // Option to log out
        Console.WriteLine("| 0. Quit                 |");
        Console.WriteLine("+=========================+");
        Console.WriteLine("| Select an option:       |");
    }

    public async Task AskUser ()
    {
        bool continueMenu = true;
        while (continueMenu)
        {
            var response = GetInputAsString();

            switch (response)
            {
                case ("1"):
                case ("customer"):
                case ("c"):
                    await _customerMenu.Menu(_currentCustomer);
                    break;
                case ("2"):
                case ("booking"):
                case ("b"):
                    await _bookingMenu.Menu(_currentCustomer);
                    break;
                case ("3"):
                case ("login"):
                case ("l"):
                    await Login(); // Call the Login method
                    break;
                case ("4"):
                case ("logout"):
                case ("lo"):
                    Logout(); // Call the Logout method
                    break;
                case ("0"):
                case ("quit"):
                case ("q"):
                    Console.WriteLine("+=====---Quitting---======+");
                    continueMenu = false;
                    break;
            }
            if (continueMenu)
            {
                PrintMenu();
            }
        }
    }

    public async Task<Customer> Login()
    {
        // Check if a customer is already logged in
        if (_currentCustomer != null)
        {
            Console.WriteLine($"You are already logged in as {_currentCustomer.firstname} {_currentCustomer.lastname}.");
            return _currentCustomer; // Return the already logged-in customer
        }

        Console.Write("| Please enter your email: ");
        var email = GetInputAsString();

        // Validate the email
        var isValid = await _query.ValidateEmail(email);
        if (isValid)
        {
            var customer = await _query.GetCustomer(email);
            if (customer != null)
            {
                _currentCustomer = customer; // Store the logged-in customer
                Console.WriteLine($"Welcome back {customer.firstname} {customer.lastname}!");
                Console.ReadLine();
                return customer; // Return the customer object
            }
        }
        else
        {
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| Email not found.                  |");
            Console.WriteLine("| Let's get you registered!         |");
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("[Press any button to continue]");
            Console.ReadLine(); // pause
            await _customerMenu.RegisterCustomer(email);
        }
        return null; // Return null if login fails
    }

    public void Logout()
    {
        if (_currentCustomer != null)
        {
            Console.WriteLine($"Goodbye, {_currentCustomer.firstname} {_currentCustomer.lastname}. You have been logged out.");
            _currentCustomer = null; // Clear the logged-in customer
        }
        else
        {
            Console.WriteLine("No user is currently logged in.");
        }

        Console.ReadLine();
    }
    
}