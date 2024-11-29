using System.Runtime.InteropServices.JavaScript;

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
        if (email is not null)
        {
            //handles email input
            var isValid = await _query.ValidateEmail(email);
            if (isValid)
            {
                Console.WriteLine("Email is valid.");
                PrintCustomerMenu();
                AskUser(email);
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

    private async void AskUser(String email)
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
                    await _mainMenu.Menu();
                    break;
            }
        }
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
        Console.WriteLine("Date of birth");
        var birthdate = DateTime.Parse(Console.ReadLine());
        if (firstName is not null && lastName is not null && phone is not null)
        {
            _query.RegisterCustomer(firstName, lastName, email, phone, birthdate);
        }

        PrintCustomerMenu();
    }

    private void ModifyCustomer(String email)
    {
        Console.WriteLine("Please fill in your new:");
        Console.WriteLine("First name");
        var firstName = Console.ReadLine();
        Console.WriteLine("Last name");
        var lastName = Console.ReadLine();
        Console.WriteLine("Phone number");
        var phone = Console.ReadLine();
        Console.WriteLine("Date of birth");
        var birthdate = DateTime.Parse(Console.ReadLine());
        if (firstName is not null && lastName is not null && phone is not null)
        {
            _query.ModifyCustomer(firstName, lastName, email, phone, birthdate);
        }
    }

    private void DeleteCustomer(String email)
    {
        _query.DeleteCustomer(email);
        Console.WriteLine("Your account has successfully been deleted");
        _mainMenu.Menu();
    }

    private void MyBookings(String email)
    {
    }
}