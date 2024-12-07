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

    // method for getting a customer object 
    public async Task<Customer?> GetCustomer(string email)
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
                    return new Customer
                    {
                        id = reader.GetInt32(0),
                        firstname = reader.GetString(1),
                        lastname = reader.GetString(2),
                        email = reader.GetString(3),
                        phone = reader.GetString(4),
                        birth = reader.GetDateTime(5)
                    };
                }
            }
        }

        return null;
    }


    // Customer menu methods

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


    // Booking menu methods

    // List available rooms within two dates, based on preferences
    public async Task ListAvailableRooms(BookingPreferences preferences)
    {
        try
        {
            //storing query
            var query =
                "SELECT room_id, hotel_id, price, room_description, balcony, size, hotel_name, hotel_description, pool, entertainment, kidsclub, restaurant, beach_proximity, city_proximity, address, city, country, average_rating " +
                "FROM room_master2 " +
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

            // The EXCEPT part will exclude the result from the second select-statment and therefor exclude all unavalable rooms
            query += " EXCEPT " +
                     "SELECT room_id, hotel_id, price, room_description, balcony, size, hotel_name, hotel_description, pool, entertainment, kidsclub, restaurant, beach_proximity, city_proximity, address, city, country, average_rating " +
                     "FROM booking_master2 " +
                     $"WHERE (booking_start_date, booking_end_date) overlaps (date '{preferences.CheckInDate.Date}', date '{preferences.CheckOutDate.Date}')";
            // The $1 is meant to be used instead of preferences.CheckInDate, this solution is a bad one because this way of doing it is not going to work in the future
            // This way is not going to be supported in the future therefor is it bad to teach it
            // I don't see how to solve this in the correct way (using $1 and $2) 

            //Adds an ORDER BY after all booleans are handled
            if (preferences.Preference.Contains("price"))
            {
                query += " ORDER BY price";
            }
            else if (preferences.Preference.Contains("rating") || preferences.Preference.Contains("review"))
            {
                query += " ORDER BY average_rating DESC ";
            }

            //query is passed into _db.CreateCommand
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
                    Console.WriteLine(new string('-', 155));
                    Console.WriteLine(
                        $"{"Room ID",-10}{"Price",-10}{"Description",-58}{"Hotel ID",-12}{"Hotel",-23}{"Amenities",-15}{"Rating",-12}{"City",-10}"
                    );
                    Console.WriteLine(new string('-', 155)); 

                    // Print table rows
                    while (await reader.ReadAsync())
                    {
                        string pool = " ";
                        string entertainment = " ";
                        string kidsclub = " ";
                        string restaurant = " ";
                        if (reader.GetBoolean(8))
                        {
                            pool = "P";
                        }

                        if (reader.GetBoolean(9))
                        {
                            entertainment = "E";
                        }

                        if (reader.GetBoolean(10))
                        {
                            kidsclub = "K";
                        }

                        if (reader.GetBoolean(11))
                        {
                            restaurant = "R";
                        }

                        // copy-paste the padding of reader titles:
                        Console.WriteLine(
                            $"{reader.GetInt32(0),-10}{reader.GetInt32(2),-10}{reader.GetString(3),-60}{reader.GetInt32(1),-10}{reader.GetString(6),-25}{pool + entertainment + restaurant + kidsclub,-15}{reader.GetFloat(17),-10}{reader.GetString(15),-10}"
                        );
                        Console.WriteLine(new string('-', 155));
                    }
                    if (!reader.HasRows)
                    {
                        Console.WriteLine("There is no available rooms matching your preferences.");
                    }
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Something went wrong with while looking at rooms.. {e.Message}");
        }
    }

    // List all booked rooms within two dates, based on preferences
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
            if (preferences.Pool) // if Pool is true
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
                query += " ORDER BY price";
            }
            else if (preferences.Preference.Contains("rating") || preferences.Preference.Contains("review"))
            {
                query += " ORDER BY average_rating DESC ";
            }

            //query is passed into _db.CreateCommand
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
                    Console.WriteLine("Booked Rooms:");
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
            Console.WriteLine($"Something went wrong with the bookings.. {e.Message}");
        }
    }


    // Booking Methods

    // Finds the higest id value in the booking table and return the next expected id (id + 1)
    public async Task<int?> GetBookingId()
    {
        await using (var cmd = _db.CreateCommand("SELECT MAX(id) FROM booking LIMIT 1"))
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

            string bookingQuery = "INSERT INTO booking (id, customer_id, start_date, end_date, extra_bed, breakfast) " +
                                  "VALUES (" + bookingId + ", $1, $2, $3, $4, $5)";

            await using (var cmd = _db.CreateCommand(bookingQuery))
            {
                cmd.Parameters.AddWithValue(customerId); // $1
                cmd.Parameters.AddWithValue(preferences.CheckInDate); // $2
                cmd.Parameters.AddWithValue(preferences.CheckOutDate); // $3
                cmd.Parameters.AddWithValue(extraBed); // $4
                cmd.Parameters.AddWithValue(dailyBreakfast); // $5

                await cmd.ExecuteNonQueryAsync();
            }

            // Add into booking_x_rooms as well
            var bookingXroomsQuery = "INSERT INTO booking_x_rooms (booking_id, room_id)" +
                                     "VALUES (" + bookingId + ", $1)";
            await using (var cmd = _db.CreateCommand(bookingXroomsQuery))
            {
                cmd.Parameters.AddWithValue(roomId); // $1

                await cmd.ExecuteNonQueryAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error while booking the room: {e.Message}");
        }
    }

    public async Task FindBooking(int bookingId)
    {
        var query = "SELECT * FROM mybookings WHERE booking_id = $1";
        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(bookingId); // $1

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                Console.Clear();
                Console.WriteLine("Your current booking:");
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
                    Console.WriteLine(new string('-', 130)); // calc ~line with comvined padding width
                }
            }
        }
    }        // Min ändring
    
    public async Task ModifyBooking(int bookingId, bool extraBed, bool dailyBreakfast)
    {
        await using (var cmd = _db.CreateCommand(
                         "UPDATE booking SET extra_bed = $1, breakfast = $2 WHERE id = $3"))
        {
            cmd.Parameters.AddWithValue(extraBed);
            cmd.Parameters.AddWithValue(dailyBreakfast);
            cmd.Parameters.AddWithValue(bookingId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
    

    public async Task DeleteBooking(int bookingId)
    {
        try
        {
            var bookingxroomsQuery = "DELETE FROM booking_x_rooms WHERE booking_id = $1";
            var bookingQuery = "DELETE FROM booking WHERE id = $1";

            await using (var cmd = _db.CreateCommand(bookingxroomsQuery))
            {
                cmd.Parameters.AddWithValue(bookingId); // $1

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

    // My bookings method

    public async Task MyBookings(int customerId)
    {
        var query = "SELECT * FROM mybookings WHERE customer_id = $1";
        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(customerId);
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                Console.Clear();
                Console.WriteLine("+===================================+");
                Console.WriteLine("|           YOUR BOOKINGS           |");
                Console.WriteLine("+===================================+\n");
                // String formatting 
                Console.WriteLine(
                    $"{"B.Id",-8}{"C.Id",-8}{"Customer",-20}{"Dates",-32}" +
                    $"{"Hotel",-23}{"Extra Bed",-12}{"Breakfast",-13}{"Price/Night",-16}" +
                    $"{"Room Size",-15}{"City",-20}");
                Console.WriteLine(new string('-', 157)); // calc ~line with comvined padding width

                while (await reader.ReadAsync())
                {
                    // Print-rows 13
                    Console.WriteLine(
                        $"{reader.GetInt32(0),-8}{reader.GetInt32(1),-8}{reader.GetString(2),-20}" +
                        $"{reader.GetDateTime(3).ToString("yy-MM-dd")} to {reader.GetDateTime(4).ToString("yy-MM-dd"),-19}" +
                        $"{reader.GetString(13),-25}{reader.GetBoolean(5),-12}{reader.GetBoolean(6),-13}{reader.GetInt32(8),-16}" +
                        $"{reader.GetInt32(11),-14}{reader.GetString(15),-20}");
                    Console.WriteLine(new string('-', 157)); // calc ~line with comvined padding width
                }

                if (!reader.HasRows)
                {
                    Console.WriteLine("You don't have any bookings");
                }
            }
        }
    }
}