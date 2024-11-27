namespace HelloHoliday;

public class CustomerMenu
{
    Query _query;
    MainMenu _mainMenu;

    public CustomerMenu(Query query, MainMenu mainMenu)
    {
        _query = query;
        _mainMenu = mainMenu;
    }

    public async Task Menu()
    {
        Console.WriteLine("What's your Email?");
        var email = Console.ReadLine();
        if(email is not null){ //handles email input
            var isValid = await _query.ValidateEmail(email);
            if(isValid)
            {
                Console.WriteLine("Email is valid.");
                bool running = true;
                while (running)
                {
                    PrintCustomerMenu();
                    running = AskUser(email);
                }
                
            }
            else
            {
                Console.WriteLine("Email not found.");
                Console.WriteLine("Let's get you registered!");
                RegisterCustomer(email);
            }
        }
    }

    private void PrintCustomerMenu()
    {
        Console.WriteLine("Choose option");
        Console.WriteLine("2. Modify");
        Console.WriteLine("3. Delete");
        Console.WriteLine("4. My Bookings");
        Console.WriteLine("0. Return to Main Menu");
    }

    private bool AskUser(String email)
    {
        var response = Console.ReadLine();
        if (response is not null)
        {
            switch (response)
            {
                case "2":
                    ModifyCustomer(email);
                    break;
                case "3":
                    DeleteCustomer(email);
                    break;
                case "4":
                    MyBookings(email);
                    break;
                case "0":
                    return false;
            }
        }

        return true;
    }

    private void RegisterCustomer(String email)
    {
        //Console.Clear();
        Console.WriteLine("Please fill in your:");
        Console.WriteLine("First name");
        var firstName = Console.ReadLine();
        Console.WriteLine("Last name");
        var lastName = Console.ReadLine();
        Console.WriteLine("Phone number");
        var phone = Console.ReadLine();
       if(firstName is not null && lastName is not null && phone is not null){
           _query.RegisterCustomer(firstName, lastName, email, phone);
       }
    }

    private void ModifyCustomer(String email)
    {
        _query.ModifyCustomer(email);
    }

    private void DeleteCustomer(String email)
    {
        if (_query.DeleteCustomer(email))
        {
            Console.WriteLine("Your account has successfully been deleted");
        }
        else
        {
            Console.WriteLine("Your account didn't get deleted");
        }
    }

    private void MyBookings(String email)
    {
      
    }
}