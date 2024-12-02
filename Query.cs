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

    public async Task GetCustomer(string email)

    {

        var query = "SELECT * FROM customer " +
                    "WHERE email = $1"; 
        
        
        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(email);
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        $"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)} \t email: {reader.GetString(3)}");
                }
            }
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
                        await GetCustomer(email);
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
            
            //storing ORDER BY preference of price or review            
            var preferenceOrder = preferences.Preference;
            
            //storing query
            var query = "SELECT * FROM booking_master " +
                        "WHERE (booking_end_date IS NULL OR booking_start_date IS NULL OR " +
                        "       booking_end_date <= $1 OR booking_start_date >= $2)" +
                        "AND (beach_proximity <= $3)" +
                        "AND (city_proximity <= $4)" +
                        "AND (size = $5)";
                        
            // Handle Booleans to display both values if false:
            if (isPool) // if isPool is true
            {
                query += "AND (pool = $3)"; //add this line to query
            }
            if (isEntertainment) 
            {
                query += "AND (entertainment = $7)"; 
            }
            if (isKidsClub) 
            {
                query += "AND (kidsclub = $8)"; 
            }
            if (isRestaurant) 
            {
                query += "AND (restaurant = $9)"; 
            }
                
                
                //Adds an ORDER BY after all booleans are handled
                if (preferenceOrder.Contains("price"))
                {
                    query +=  " ORDER BY price"; // working
                }
                else if (preferenceOrder.Contains("rating"))
                {
                    query += " ORDER BY average_rating"; // NOT WORKING
                }
            
            //query passed into _db.CreateCommand
            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckOutDate));       // $1
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckInDate));        // $2
                
                cmd.Parameters.AddWithValue(Int32.Parse(preferences.DistanceToBeach));       // $3
                cmd.Parameters.AddWithValue(Int32.Parse(preferences.DistanceToCityCentre));  // $4
                cmd.Parameters.AddWithValue(Int32.Parse(preferences.RoomSize));              // $5
                
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Pool));                // $6  
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Entertainment));       // $7  
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.KidsClub));            // $8  
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Restaurant));          // $9  


                cmd.Parameters.AddWithValue(preferences.Preference);                         // $10
                
               
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
                            $"rating: {reader.GetFloat(12)} \t" +
                            $"description: {reader.GetString(3)} \t " +
                            $"date: {reader.GetDateTime(13).ToString("yy-MM-dd")} to:{reader.GetDateTime(14).ToString("yy-MM-dd")}"
                        );
                    }
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong with the inputs.. {e.Message}");
        }
    }
    
    
    // List available rooms within two dates,  based on preferences
    public async Task ListAvaliableRooms(BookingPreferences preferences)
    {
        try
        {
            //storing bool values:
            var isPool = Boolean.Parse(preferences.Pool);
            var isEntertainment = Boolean.Parse(preferences.Entertainment);
            var isKidsClub = Boolean.Parse(preferences.KidsClub);
            var isRestaurant = Boolean.Parse(preferences.Restaurant);
            
            //storing ORDER BY preference of price or review            
            var preferenceOrder = preferences.Preference;
            
            //storing query
            var query = "SELECT * FROM booking_master " +
                        "WHERE (booking_start_date <= $2 AND booking_end_date >= $1) " +
                        "AND (beach_proximity <= $3)" +
                        "AND (city_proximity <= $4)" +
                        "AND (size = $5)";
                        
            // Handle Booleans to display both values if false:
            if (isPool) // if isPool is true
            {
                query += "AND (pool = $6)"; //add this line to query
            }
            if (isEntertainment) 
            {
                query += "AND (entertainment = $7)"; 
            }
            if (isKidsClub) 
            {
                query += "AND (kidsclub = $8)"; 
            }
            if (isRestaurant) 
            {
                query += "AND (restaurant = $9)"; 
            }
                
                
                //Adds an ORDER BY after all booleans are handled
                if (preferenceOrder.Contains("price"))
                {
                    query +=  " ORDER BY price"; // working
                }
                else if (preferenceOrder.Contains("average_rating"))
                {
                    query += " ORDER BY rating"; // NOT WORKING
                }
            
            //query passed into _db.CreateCommand
            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckOutDate));       // $1
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckInDate));        // $2
                
                cmd.Parameters.AddWithValue(Int32.Parse(preferences.DistanceToBeach));       // $3
                cmd.Parameters.AddWithValue(Int32.Parse(preferences.DistanceToCityCentre));  // $4
                cmd.Parameters.AddWithValue(Int32.Parse(preferences.RoomSize));              // $5
                
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Pool));                // $6  
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Entertainment));       // $7  
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.KidsClub));            // $8  
                cmd.Parameters.AddWithValue(Boolean.Parse(preferences.Restaurant));          // $9  


                cmd.Parameters.AddWithValue(preferences.Preference);                         // $10
                
               
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine("Available Rooms:");
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
                            $"rating: {reader.GetFloat(12)} \t" +
                            $"description: {reader.GetString(3)} \t " +
                            $"date: {reader.GetDateTime(13).ToString("yy-MM-dd")} to:{reader.GetDateTime(14).ToString("yy-MM-dd")}"
                        );
                    }
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong with the inputs.. {e.Message}");
        }
    }
    
    // Ask follow-up questions:
    // Would you like to book a room based on your search?
     // > Enter the room id of your choice
    // Would you like to add an extra bed for 30$?
    // > y/n
    // Would you like to include breakfast?
    // > y/n
   
    // Booking Method
    public async Task BookRoom(BookingPreferences preferences, int roomId, string email)
    {
        try
        { 
                                                            // Add into bookingXrooms as well 
            var bookingQuery = "INSERT INTO bookings (customer_id, room_id, booking_start_date, booking_end_date, extra_bed, breakfast) " +
                               "VALUES ($1, $2, $3, $4)";

            await using (var cmd = _db.CreateCommand(bookingQuery))
            {
                // Find id based on email
                //var customerId = email;

               // cmd.Parameters.AddWithValue(customerId);                               // $1
                cmd.Parameters.AddWithValue(roomId);                                   // $2
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckInDate));  // $3
                cmd.Parameters.AddWithValue(DateTime.Parse(preferences.CheckOutDate)); // $4
               // cmd.Parameters.AddWithValue(Boolean.Parse(preferences.extra_bed));     // $5
                //cmd.Parameters.AddWithValue(Boolean.Parse(preferences.breakfast));     // $6
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while booking the room: {e.Message}");
        }
    }
    
    
}
    
    




