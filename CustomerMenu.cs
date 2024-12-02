using System.Runtime.InteropServices.JavaScript;

namespace HelloHoliday;

public class CustomerMenu
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
        Console.WriteLine("What's your Email?");
        var email = Console.ReadLine();
        if (email is not null)
        {
            //handles email input
            var isValid = await _query.ValidateEmail(email);
            if (isValid)
            {
                Console.WriteLine("Email is valid.");
                _customer = await _query.GetCustomer(email);
                PrintCustomerMenu();
                await AskUser();
            }
            else
            {
                Console.WriteLine("Email not found.");
                Console.WriteLine("Let's get you registered!");
                await RegisterCustomer(email);
            }
        }
    }

    private void PrintCustomerMenu()
    {
        Console.WriteLine("### Customer Menu");
        Console.WriteLine("1. My Bookings");
        Console.WriteLine("2. Modify");
        Console.WriteLine("3. Delete");
        Console.WriteLine("0. Return to Main Menu");
    }

    private async Task AskUser()
    {
        var response = Console.ReadLine();
        if (response is not null)
        {
            switch (response)
            {
                case "1":
                    MyBookings();
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
    }

    private async Task RegisterCustomer(string email)
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
            _customer = await _query.GetCustomer(email);
        }

        PrintCustomerMenu();
        await AskUser();
    }

    // Switch-case? User should also be able to modify email..
    private async Task ModifyCustomer()
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
            await _query.ModifyCustomer(firstName, lastName, _customer.email, phone, birthdate);
            
        }
    }

    private async Task DeleteCustomer()
    {
        await _query.DeleteCustomer(_customer.id);
        Console.WriteLine("Your account has successfully been deleted");
        await _mainMenu.Menu();
    }

    private void MyBookings()
    {
        
    }
}