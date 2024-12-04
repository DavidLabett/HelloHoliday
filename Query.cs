using System.Runtime.InteropServices.JavaScript;
using Npgsql;
using Exception = System.Exception;

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

    public async Task<Customer> GetCustomer(string email)
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
                    int customerId = reader.GetInt32(0);
                    string customerFirstName = reader.GetString(1);
                    string customerLastName = reader.GetString(2);
                    string customerEmail = reader.GetString(3);
                    string customerPhone = reader.GetString(4);
                    DateTime customerBirth = reader.GetDateTime(5);
                    //Console.WriteLine($"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)} \t email: {reader.GetString(3)}");
                    //Console.WriteLine(
                    //$"{customerId}, {customerFirstName}, {customerFirstName}, {customerEmail}, {customerPhone}, {customerBirth}");
                    return new Customer
                    {
                        id = customerId,
                        firstname = customerFirstName,
                        lastname = customerLastName,
                        email = customerEmail,
                        phone = customerPhone,
                        birth = customerBirth
                    };
                }
            }
        }

        return null;
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
                        return true;
                    }
                    else
                    {
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

    public async Task DeleteCustomer(int id)
    {
        await using (var cmd = _db.CreateCommand("DELETE FROM customer WHERE id = $1"))
        {
            cmd.Parameters.AddWithValue(id);
            await cmd.ExecuteNonQueryAsync();
        }
    }


    //BookingQueriescs

    // Method to handle all BookingPreferences generated by BookingMenu
    public async Task ListBookingPref(BookingPreferences preferences)
    {
        try
        {
            //storing query
            var query = "SELECT * FROM booking_master " +
                        "WHERE (booking_end_date IS NULL OR booking_start_date IS NULL OR " +
                        "       booking_end_date <= $1 OR booking_start_date >= $2)" +
                        "AND (beach_proximity <= $3)" +
                        "AND (city_proximity <= $4)" +
                        "AND (size = $5)";

            // Handle Booleans to display both values if false:
            if (preferences.Pool) // if isPool is true
            {
                query += "AND (pool = $6)"; //add this line to query
            }

            if (preferences.Entertainment)
            {
                query += "AND (entertainment = $7)";
            }

            if (preferences.KidsClub)
            {
                query += "AND (kidsclub = $8)";
            }

            if (preferences.Restaurant)
            {
                query += "AND (restaurant = $9)";
            }


            //Adds an ORDER BY after all booleans are handled
            if (preferences.Preference.Contains("price"))
            {
                query += " ORDER BY (price)"; // working
            }
            else if (preferences.Preference.Contains("rating"))
            {
                query += " ORDER BY (average_rating) DESC"; // NOT WORKING
            }

            //query passed into _db.CreateCommand
            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(preferences.CheckOutDate); // $1
                cmd.Parameters.AddWithValue(preferences.CheckInDate); // $2
                cmd.Parameters.AddWithValue(preferences.DistanceToBeach); // $3
                cmd.Parameters.AddWithValue(preferences.DistanceToCityCentre); // $4
                cmd.Parameters.AddWithValue(preferences.RoomSize); // $5
                cmd.Parameters.AddWithValue(preferences.Pool); // $6  
                cmd.Parameters.AddWithValue(preferences.Entertainment); // $7  
                cmd.Parameters.AddWithValue(preferences.KidsClub); // $8  
                cmd.Parameters.AddWithValue(preferences.Restaurant); // $9  
                cmd.Parameters.AddWithValue(preferences.Preference); // $10
                
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

    public async Task ListAvailableRooms(BookingPreferences preferences)
    {
        /*
        select *
            from room_master
            except
        select room_id, hotel_id, price, description, balcony,
            size, pool, entertainment, kidsclub, restaurant,
            beach_proximity, city_proximity, average_rating
        from booking_master
        where booking_start_date between '2024-12-1' and '2024-12-10'
        and booking_end_date between '2024-12-1' and '2024-12-10'
        order by price;
        */

        try
        {
            //storing query
            var query = "SELECT room_id, hotel_id, price, description, balcony, size, pool, entertainment, kidsclub, restaurant, beach_proximity, city_proximity, average_rating FROM room_master " +
                        "WHERE (beach_proximity <= $3)" +
                        "AND (city_proximity <= $4)" +
                        "AND (size = $5)";

            // Handle Booleans to display both values if false:
            if (preferences.Pool) // if isPool is true
            {
                query += "AND (pool = $6)"; //add this line to query
            }

            if (preferences.Entertainment)
            {
                query += "AND (entertainment = $7)";
            }

            if (preferences.KidsClub)
            {
                query += "AND (kidsclub = $8)";
            }

            if (preferences.Restaurant)
            {
                query += "AND (restaurant = $9)";
            }

            query += "EXCEPT " +
                     "SELECT room_id, hotel_id, price, description, balcony, size, pool, entertainment, kidsclub, restaurant, beach_proximity, city_proximity, average_rating " +
                     "FROM booking_master " +
                     "WHERE (booking_start_date BETWEEN $1 AND $2) " + 
                     "AND (booking_end_date BETWEEN $1 AND $2) ";

            //Adds an ORDER BY after all booleans are handled
            if (preferences.Preference.Contains("price"))
            {
                query += " ORDER BY price"; // working
            }
            else if (preferences.Preference.Contains("average_rating"))
            {
                query += " ORDER BY rating"; // NOT WORKING
            }

            //query passed into _db.CreateCommand
            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(preferences.CheckInDate); // $1
                cmd.Parameters.AddWithValue(preferences.CheckOutDate); // $2

                cmd.Parameters.AddWithValue(preferences.DistanceToBeach); // $3
                cmd.Parameters.AddWithValue(preferences.DistanceToCityCentre); // $4
                cmd.Parameters.AddWithValue(preferences.RoomSize); // $5

                cmd.Parameters.AddWithValue(preferences.Pool); // $6  
                cmd.Parameters.AddWithValue(preferences.Entertainment); // $7  
                cmd.Parameters.AddWithValue(preferences.KidsClub); // $8  
                cmd.Parameters.AddWithValue(preferences.Restaurant); // $9  


                cmd.Parameters.AddWithValue(preferences.Preference); // $10


                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine("Available Rooms:");
                    //Reader titles for each column, with string formatting, {String, padding}
                    Console.WriteLine(
                        $"{"Room ID",-10}{"Hotel ID",-10}{"Price",-10}{"Rating",-10}" +
                        $"{"Description",-60}{"Pool",-10}{"Entertainment",-15}{"Kids Club",-15}{"Restaurant",-15}");
                    // writes string with '-' count ~= 155 (all padding combined to match width)
                    Console.WriteLine(new string('-', 155)); 
    
                    // Print table rows
                    while (await reader.ReadAsync())
                    {
                        // copy-paste the padding of reader titles:
                        Console.WriteLine(
                            $"{reader.GetInt32(0),-10}{reader.GetInt32(1),-10}{reader.GetInt32(2),-10}{reader.GetFloat(12),-10}" +
                            $"{reader.GetString(3),-60}{reader.GetBoolean(6),-10}{reader.GetBoolean(7),-15}{reader.GetBoolean(8),-15}{reader.GetBoolean(9),-15}");
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
    public async Task ListBookedRooms(BookingPreferences preferences)
    {
        try
        {
            //storing query
            var query = "SELECT * FROM booking_master " +
                        "WHERE (booking_start_date <= $2 AND booking_end_date >= $1) " +
                        "AND (beach_proximity <= $3)" +
                        "AND (city_proximity <= $4)" +
                        "AND (size = $5)";

            // Handle Booleans to display both values if false:
            if (preferences.Pool) // if isPool is true
            {
                query += "AND (pool = $6)"; //add this line to query
            }

            if (preferences.Entertainment)
            {
                query += "AND (entertainment = $7)";
            }

            if (preferences.KidsClub)
            {
                query += "AND (kidsclub = $8)";
            }

            if (preferences.Restaurant)
            {
                query += "AND (restaurant = $9)";
            }


            //Adds an ORDER BY after all booleans are handled
            if (preferences.Preference.Contains("price"))
            {
                query += " ORDER BY price"; // working
            }
            else if (preferences.Preference.Contains("average_rating"))
            {
                query += " ORDER BY rating"; // NOT WORKING
            }

            //query passed into _db.CreateCommand
            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(preferences.CheckOutDate); // $1
                cmd.Parameters.AddWithValue(preferences.CheckInDate); // $2

                cmd.Parameters.AddWithValue(preferences.DistanceToBeach); // $3
                cmd.Parameters.AddWithValue(preferences.DistanceToCityCentre); // $4
                cmd.Parameters.AddWithValue(preferences.RoomSize); // $5

                cmd.Parameters.AddWithValue(preferences.Pool); // $6  
                cmd.Parameters.AddWithValue(preferences.Entertainment); // $7  
                cmd.Parameters.AddWithValue(preferences.KidsClub); // $8  
                cmd.Parameters.AddWithValue(preferences.Restaurant); // $9  


                cmd.Parameters.AddWithValue(preferences.Preference); // $10


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

    
    // Booking Method

    public async Task<int?> GetBookingId()
    {
        await using (var cmd = _db.CreateCommand("select MAX(id) from booking limit 1"))
        {
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    return reader.GetInt32(0) + 1;
                }
            }
        }

        return null;
    }

    public async Task BookRoom(BookingPreferences preferences, int customerId, int roomId, bool extraBed,
        bool dailyBreakfast)
    {
        try
        {
            int? bookingId = await GetBookingId();
            string bookingQuery;

            bookingQuery = "INSERT INTO booking (id, customer_id, start_date, end_date, extra_bed, breakfast) " +
                           "VALUES (" + bookingId + ", $1, $2, $3, $4, $5)";


            await using (var cmd = _db.CreateCommand(bookingQuery))
            {
                // Find id based on email
                //var customerId = email;

                cmd.Parameters.AddWithValue(customerId); // $1
                //cmd.Parameters.AddWithValue(roomId);                                   // $2
                cmd.Parameters.AddWithValue(preferences.CheckInDate); // $3
                cmd.Parameters.AddWithValue(preferences.CheckOutDate); // $4
                cmd.Parameters.AddWithValue(extraBed); // $5
                cmd.Parameters.AddWithValue(dailyBreakfast); // $6

                await cmd.ExecuteNonQueryAsync();
            }

            // Add into bookingXrooms as well
            var bookingXroomsQuery = "INSERT INTO booking_x_rooms (booking_id, room_id)" +
                                     "VALUES (" + bookingId + ", $1)";
            await using (var cmd = _db.CreateCommand(bookingXroomsQuery))
            {
                cmd.Parameters.AddWithValue(roomId); //$1

                await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while booking the room: {e.Message}");
        }
    }


    /*  public async Task ModifyBooking(BookingId)
    {
        int? bookingId = await GetBookingId();
    }
    */

    public async Task DeleteBooking(int bookingId)
    {
        try
        {
            var bookingxroomsQuery = "DELETE from booking_x_rooms where id = $1";
            var bookingQuery = "DELETE from booking where id = $1";

            await using (var cmd = _db.CreateCommand(bookingxroomsQuery))
            {
                cmd.Parameters.AddWithValue(bookingId);

                await cmd.ExecuteReaderAsync();
            }

            await using (var cmd = _db.CreateCommand(bookingQuery))
            {
                cmd.Parameters.AddWithValue(bookingId); // $1

                await cmd.ExecuteReaderAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }


    public async Task MyBookings(int customerId)
    {
        var query = "SELECT * FROM mybookings WHERE customer_id = $1";
        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(customerId);
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                Console.Clear();
                Console.WriteLine("+=============================================+");
                Console.WriteLine("|                 YOUR BOOKINGS               |");
                Console.WriteLine("+=============================================+");
               // String formatting
                Console.WriteLine(
                    $"{"B.Id",-8}{"C.Id",-8}{"Customer",-20}{"Dates",-30}" +
                    $"{"Extra Bed",-12}{"Breakfast",-12}{"Price/Night",-15}" +
                    $"{"Room Size",-15}{"City",-20}");
                Console.WriteLine(new string('-', 130)); // calc ~line with comvined padding width

                while (await reader.ReadAsync())
                {
                    // Print-rows
                    Console.WriteLine(
                        $"{reader.GetInt32(0),-8}{reader.GetInt32(1),-8}{reader.GetString(2),-20}" +
                        $"{reader.GetDateTime(3).ToString("yy-MM-dd")} to {reader.GetDateTime(4).ToString("yy-MM-dd"),-19}" +
                        $"{reader.GetBoolean(5),-12}{reader.GetBoolean(6),-12}{reader.GetInt32(8),-15}" +
                        $"{reader.GetInt32(11),-14}{reader.GetString(15),-20}");
                }
            }
        }
    }

}