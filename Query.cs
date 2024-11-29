using System.Runtime.InteropServices.JavaScript;
using Npgsql;

namespace HelloHoliday;

public class Query
{
    NpgsqlDataSource _db;

    public Query(NpgsqlDataSource db)
    {
        _db = db;
    }

    public async void ListAll()
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM items"))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Console.WriteLine($"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)}");
            }
        }
    }

    public async void ShowOne(string id)
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM items WHERE id = $1"))
        {
            cmd.Parameters.AddWithValue(int.Parse(id));
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        $"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)} \t slogan: {reader.GetString(2)}");
                }
            }
        }
    }

    public async void AddOne(string name, string? slogan)
    {
        // Insert data
        await using (var cmd = _db.CreateCommand("INSERT INTO items (name, slogan) VALUES ($1, $2)"))
        {
            cmd.Parameters.AddWithValue(name);
            cmd.Parameters.AddWithValue(slogan);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async void UpdateOne(string id)
    {
        Console.WriteLine("Current entry:");
        ShowOne(id);
        Console.WriteLine("Enter updated name (required)");
        var name = Console.ReadLine(); // required
        Console.WriteLine("Enter updated slogan");
        var slogan = Console.ReadLine(); // not required
        if (name is not null)
        {
            // Update data
            await using (var cmd = _db.CreateCommand("UPDATE items SET name = $2, slogan = $3 WHERE id = $1"))
            {
                cmd.Parameters.AddWithValue(int.Parse(id));
                cmd.Parameters.AddWithValue(name);
                cmd.Parameters.AddWithValue(slogan);
                await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async void DeleteOne(string id)
    {
        // Delete data
        await using (var cmd = _db.CreateCommand("DELETE FROM items WHERE id = $1"))
        {
            cmd.Parameters.AddWithValue(int.Parse(id));
            await cmd.ExecuteNonQueryAsync();
        }
    }

    // Customer menu metoder
    public async Task<bool> ValidateEmail(String email)
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM customer WHERE email = $1"))
        {
            cmd.Parameters.AddWithValue(email);
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    string dbEmail = reader.GetString(3);
                    //Console.WriteLine($"dbEmail: {reader.GetString(0)} \t email: {email}");
                    if (email == dbEmail)
                    {
                        Console.WriteLine("Welcome!");
                        return true;
                    }
                    else
                    {
                        Console.WriteLine("Invalid email");
                        return false;
                    }
                }

                return false;
            }
        }
    }

    public async Task RegisterCustomer(String firstName, String lastName, String email, String phone, DateTime birth)
    {
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO customer (firstname, lastname, email, phone, birth) VALUES ($1, $2, $3, $4, $5)"))
        {
            cmd.Parameters.AddWithValue(firstName);
            cmd.Parameters.AddWithValue(lastName);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phone);
            cmd.Parameters.AddWithValue(birth);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task ModifyCustomer(String firstName, String lastName, String email, String phone, DateTime birth)
    {
        await using (var cmd = _db.CreateCommand(
                         "UPDATE customer SET firstname = $1, lastname = $2, phone = $4, birth = $5 WHERE email = $3"))
        {
            cmd.Parameters.AddWithValue(firstName);
            cmd.Parameters.AddWithValue(lastName);
            cmd.Parameters.AddWithValue(email);
            cmd.Parameters.AddWithValue(phone);
            cmd.Parameters.AddWithValue(birth);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task DeleteCustomer(String email)
    {
        await using (var cmd = _db.CreateCommand("DELETE FROM customer WHERE email = $1"))
        {
            cmd.Parameters.AddWithValue(email);
            await cmd.ExecuteNonQueryAsync();
        }
    }


    //BookingQueriescs

    // Method to handle all BookingPreferences generated by BookingMenu
    public async Task ListBookingPref(BookingPreferences preferences)
    {
        try
        {
            //storing bool values:
            var isPool = Boolean.Parse(preferences.Pool);
            var isEntertainment = Boolean.Parse(preferences.Entertainment);
            var isKidsClub = Boolean.Parse(preferences.KidsClub);
            var isRestaurant = Boolean.Parse(preferences.Restaurant);

            //storing query
            var query = "SELECT * FROM booking_master " +
                        "WHERE (booking_end_date IS NULL OR booking_start_date IS NULL OR " +
                        "       booking_end_date <= $1 OR booking_start_date >= $2)";

            // Handle Booleans to display both values if false:
            if (isPool) // if isPool is true
            {
                query += "AND (pool = $3)"; //add this line to query
            }

            if (isEntertainment)
            {
                query += "AND (entertainment = $4)";
            }

            if (isKidsClub)
            {
                query += "AND (kidsclub = $5)";
            }

            if (isRestaurant)
            {
                query += "AND (restaurant = $4)";
            }

            if (preferences.Preference == "price")
            {
                query += "ORDER BY price";
            }
            else if (preferences.Preference == "reviews")
            {
                query += "ORDER BY average_rating";
            }

            //query passed into _db.CreateCommand
            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckOutDate)); // $1
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckInDate)); // $2

                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Pool)); // $3
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Entertainment)); // $4
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.KidsClub)); // $5
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Restaurant)); // $6

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(
                            //Validation that both boolean values are checked:
                            $"pool: {reader.GetBoolean(6)} \t " +
                            $"entertainment: {reader.GetBoolean(7)} \t " +
                            $"kidsclub: {reader.GetBoolean(8)} \t " +
                            $"restaurant: {reader.GetBoolean(9)} \t " +
                            //Validation that both boolean values are checked^
                            $"room_id: {reader.GetInt32(0)} \t " +
                            $"hotel_id: {reader.GetInt32(1)} \t " +
                            $"price: {reader.GetInt32(2)} \t " +
                            $"description: {reader.GetString(3)} \t " +
                            $"rating: {reader.GetFloat(12)} \t" +
                            $"date: {reader.GetDateTime(13).ToString("yy-MM-dd")} to:{reader.GetDateTime(14).ToString("yy-MM-dd")}"
                        );
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong with the inputs..");
        }
    }
}