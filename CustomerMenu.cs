namespace HelloHoliday;

public class CustomerMenu : Menu
{
    Query _query;
    MainMenu _mainMenu;
    Customer _customer;

    public CustomerMenu(Query query, MainMenu mainMenu)
    {
        _query = query;
        _mainMenu = mainMenu;
    }

    public async Task Menu()
    {
        Console.Write("| Please enter your email: ");
        var email = GetInputAsString();

        //handles email input
        var isValid = await _query.ValidateEmail(email);
        if (isValid)
        {
            
            _customer = await _query.GetCustomer(email);
            Console.Clear();
            Console.WriteLine($"Welcome back {_customer.firstname} {_customer.lastname}!");
            PrintCustomerMenu();
            await AskUser();
        }
        else
        {
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("| Email not found.                  |");
            Console.WriteLine("| Let's get you registered!         |");
            Console.WriteLine("+-----------------------------------+");
            Console.WriteLine("[Press any button to continue]");
            Console.ReadLine(); //pause
            await RegisterCustomer(email);
        }
    }

    private void PrintCustomerMenu()
    {
        Console.WriteLine("+=========================+");
        Console.WriteLine("|      CUSTOMER MENU      |");
        Console.WriteLine("+=========================+");
        Console.WriteLine("| 1. View My Bookings     |");
        Console.WriteLine("| 2. Modify Profile       |");
        Console.WriteLine("| 3. Delete Account       |");
        Console.WriteLine("| 0. Return to Main Menu  |");
        Console.WriteLine("+=========================+");
        Console.WriteLine("| Select an option:       |");
    }

    private async Task AskUser()
    {
        var response = GetInputAsString();

        switch (response)
        {
            case "1":
                await MyBookings();
                break;
            case "2":
                await ModifyCustomer();
                break;
            case "3":
                await DeleteCustomer();
                break;
            case "0":
                await _mainMenu.Menu();
                break;
        }
    }

    public async Task RegisterCustomer(string email)
    {
        Console.Clear();
        Console.WriteLine("+===================================+");
        Console.WriteLine("|         REGISTER NEW CUSTOMER     |");
        Console.WriteLine("+===================================+");

        // Collect user input with prompts
        Console.WriteLine("| Please fill in the following details:");
        Console.WriteLine("+-----------------------------------+");

        Console.Write("| First Name: ");
        var firstName = GetInputAsString();

        Console.Write("| Last Name: ");
        var lastName = GetInputAsString();

        Console.Write("| Phone Number: ");
        var phone = GetInputAsString();

        Console.Write("| Date of Birth (YYYY-MM-DD): ");
        var birthdate = GetInputAsDate();

        Console.WriteLine("+-----------------------------------+");
        Console.WriteLine("| Registering your account, please wait...");
        Console.WriteLine("+-----------------------------------+");

        // Register the customer and fetch their details
        await _query.RegisterCustomer(firstName, lastName, email, phone, birthdate);
        _customer = await _query.GetCustomer(email);

        Console.WriteLine("+===================================+");
        Console.WriteLine("| Registration Successful!          |");
        Console.WriteLine("+===================================+");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine();
        PrintCustomerMenu();
        await AskUser();
    }

    // Switch-case? User should also be able to modify email..
    private async Task ModifyCustomer()
    {
        Console.Clear();
        Console.WriteLine("+===================================+");
        Console.WriteLine("|        MODIFY CUSTOMER DETAILS    |");
        Console.WriteLine("+===================================+");

        // Display current details for reference
        Console.WriteLine("| Current Details:");
        Console.WriteLine($"| First Name:     {_customer.firstname}");
        Console.WriteLine($"| Last Name:      {_customer.lastname}");
        Console.WriteLine($"| Phone Number:   {_customer.phone}");
        Console.WriteLine($"| Date of Birth:  {_customer.birth}");
        Console.WriteLine("+-----------------------------------+");
        // New details below
        Console.WriteLine("| Please fill in your new details:");

        Console.Write("| First Name: ");
        var firstName = GetInputAsString();

        Console.Write("| Last Name: ");
        var lastName = GetInputAsString();

        Console.Write("| Phone Number: ");
        var phone = GetInputAsString();

        Console.Write("| Date of Birth (YYYY-MM-DD): ");
        var birthdate = GetInputAsDate();

        Console.WriteLine("+-----------------------------------+");
        Console.WriteLine("| Updating your information, please wait...");
        Console.WriteLine("+-----------------------------------+");

        // Update the customer's details
        await _query.ModifyCustomer(firstName, lastName, _customer.email, phone, birthdate);

        // Update the local customer object with new details
        _customer = await _query.GetCustomer(_customer.email);

        Console.WriteLine("+===================================+");
        Console.WriteLine("| Details successfully updated!     |");
        Console.WriteLine("+===================================+");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine(); //pause
        PrintCustomerMenu();
        await AskUser();
    }

    private async Task DeleteCustomer()
    {
        await _query.DeleteCustomer(_customer.id);
        Console.WriteLine("| Your account has successfully been deleted |");
        Console.WriteLine("[Press any button to continue]");
        Console.ReadLine();
        await _mainMenu.Menu();
    }

    private async Task MyBookings()
    {
        await _query.MyBookings(_customer.id);
    }
}