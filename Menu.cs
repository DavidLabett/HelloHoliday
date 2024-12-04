namespace HelloHoliday;

public abstract class Menu
{
    public string GetInputAsString()
    {
        Console.Write("> ");
        string? input = Console.ReadLine();
        input = input.Trim().ToLower();

        if (string.IsNullOrEmpty(input))
        {
            Console.WriteLine("You need to write something");
            input = GetInputAsString();
        }

        return input;
    }

    public bool GetInputAsBool()
    {
        string input = GetInputAsString();

        switch (input)
        {
            case "y":
            case "yes":
            case "true":
                return true;
            case "n":
            case "no":
            case "false":
                return false;
        }

        Console.WriteLine("Please say 'yes' or 'no'");
        return GetInputAsBool();
    }

    public int GetInputAsInt()
    {
        string input = GetInputAsString();
        int number;

        try
        {
            number = int.Parse(input);
        }
        catch (Exception e)
        {
            Console.WriteLine("Need to be a number");
            return GetInputAsInt();
        }
        return number;
    }

    public DateTime GetInputAsDate()
    {
        string input = GetInputAsString();
        DateTime date;

        try
        {
            date = DateTime.Parse(input);
        }
        catch (Exception e)
        {
            Console.WriteLine("Not a date, try yyyy-mm-dd");
            return GetInputAsDate();
        }

        return date;
    }

    public int GetInputAsRoomSize()
    {
        string input = GetInputAsString();

        switch (input)
        {
            case "single":
            case "s":
            case "1":
                return 1;
            case "double":
            case "d":
            case "2":
                return 2;
            case "triple":
            case "t":
            case "3":
                return 3;
            case "quad":
            case "q":
            case "4":
                return 4;
        }

        Console.WriteLine("That's not a room size (Single, Double, Triple, Quad)");
        return GetInputAsRoomSize();
    }
}