namespace HelloHoliday;

public class CustomerMenu
{
    Query _query;
    public CustomerMenu(Query query)
    {
        _query = query;
        PrintMenu();
    }

    private void PrintMenu()
    {
        Console.WriteLine("Choose option");
        Console.WriteLine("1. Register");
        Console.WriteLine("2. Modify");
        Console.WriteLine("3. Delete");
        Console.WriteLine("4. My Bookings");
        Console.WriteLine("0. Return to Main Menu");
        AskUser();
    }

    private void AskUser()
    {
        var response = Console.ReadLine();
        if (response is not null)
        {
            switch (response)
            {
                case "1":
                    RegisterCustomer();
                    break;
                case "2":
                    ModifyCustomer();
                    break;
                case "3":
                    DeleteCustomer();
                    break;
                case "4":
                    MyBookings();
                    break;
                case "0":
                    break;
            }
        }
    }

    private void RegisterCustomer()
    {
        Console.Clear();
        Console.WriteLine("First name");
        var firstName = Console.ReadLine();
        Console.WriteLine("Last name");
        var lastName = Console.ReadLine();
        Console.WriteLine("Email");
        var email = Console.ReadLine();
        Console.WriteLine("Phone number");
        var phone = Console.ReadLine();
        _query.RegisterCustomer(firstName, lastName, email, phone);
    }

    private void ModifyCustomer()
    {
        Console.WriteLine("Enter email");
        var email = Console.ReadLine();
        _query.ModifyCustomer(email);
    }

    private void DeleteCustomer()
    {
        Console.WriteLine("Enter email");
        var email = Console.ReadLine();
        if (_query.DeleteCustomer(email))
        {
            Console.WriteLine("Your account has successfully been deleted");
        }
        else
        {
            Console.WriteLine("Your account didn't get deleted");
        }
    }

    private void MyBookings()
    {
        
    }
}