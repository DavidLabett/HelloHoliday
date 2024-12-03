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
                    int customerId = reader.GetInt32(0);
                    string customerFirstName = reader.GetString(1);
                    string customerLastName = reader.GetString(2);
                    string customerEmail = reader.GetString(3);
                    string customerPhone = reader.GetString(4);
                    DateTime customerBirth = reader.GetDateTime(5);
                    
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
            // The EXCEPT part will exclude the result from the second select-statment and therefor exclude all unavalable rooms
            query += "EXCEPT " +
                     "SELECT room_id, hotel_id, price, description, balcony, size, pool, entertainment, kidsclub, restaurant, beach_proximity, city_proximity, average_rating " +
                     "FROM booking_master " +
                     "WHERE (booking_start_date BETWEEN $1 AND $2) " + 
                     "AND (booking_end_date BETWEEN $1 AND $2) ";

            //Adds an ORDER BY after all booleans are handled
            if (preferences.Preference.Contains("price"))
            {
                query += " ORDER BY price"; 
            }
            else if (preferences.Preference.Contains("rating") || preferences.Preference.Contains("review"))
            {
                query += " ORDER BY average_rating";
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
                            $"description: {reader.GetString(3)}"
                        );
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
                query += " ORDER BY average_rating"; 
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
    
    /*  public async Task ModifyBooking(BookingId)
    {
        int? bookingId = await GetBookingId();
    }
    */

    public async Task DeleteBooking(int bookingId)
    {
        try
        {
            var bookingxroomsQuery = "DELETE from booking_x_rooms where booking_id = $1";
            var bookingQuery = "DELETE from booking where id = $1";

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
                Console.WriteLine("+=============================================+");
                Console.WriteLine("|                 YOUR BOOKINGS               |");
                Console.WriteLine("+=============================================+");
                while (await reader.ReadAsync())
                {
                    Console.WriteLine(
                        $"B.Id: {reader.GetInt32(0)} \t " +
                        $"C.Id: {reader.GetInt32(1)} \t " +
                        $"Customer: {reader.GetString(2)} \t " +
                        $"Dates: {reader.GetDateTime(3).ToString("yy-MM-dd")} to:{reader.GetDateTime(4).ToString("yy-MM-dd")} \t " +
                        $"Extra bed: {reader.GetBoolean(5)} \t " +
                        $"Breakfast: {reader.GetBoolean(6)} \t " +
                        $"Price/night: {reader.GetInt32(8)} \t " +
                        $"Room size: {reader.GetInt32(11)} \t " +
                        $"City: {reader.GetString(15)} \t" 
                    );
                }
            }
        }
    }
}