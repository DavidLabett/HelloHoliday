using Npgsql;

namespace HelloHoliday;

public class BookingsQueriescs
{ 
    NpgsqlDataSource _db;

        public BookingsQueriescs(NpgsqlDataSource db)
        {
            _db = db;
        }

        // Method to fetch available rooms for specific dates (check-in and check-out)
        public async void FetchAvailableRooms(DateTime checkIn, DateTime checkOut)
        {
            const string query = @"
                SELECT
                    r.id AS room_id,
                    r.hotel_id,
                    r.price,
                    r.description,
                    r.balcony,
                    r.size
                FROM
                    Room r
                LEFT JOIN
                    Booking_x_rooms bxr ON r.id = bxr.room_id
                LEFT JOIN
                    Booking b ON bxr.booking_id = b.id
                WHERE
                    b.id IS NULL
                    OR (
                        b.start_date >= $1 -- Check-out date
                        OR b.start_date + INTERVAL '1 DAY' <= $2 -- Check-in date
                    );";

            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(checkOut); // $1
                cmd.Parameters.AddWithValue(checkIn);  // $2
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine("Available Rooms:");
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"Room ID: {reader.GetInt32(0)}, Hotel ID: {reader.GetInt32(1)}, Price: {reader.GetDecimal(2)}, Description: {reader.GetString(3)}, Balcony: {reader.GetBoolean(4)}, Size: {reader.GetString(5)}");
                    }
                }
            }
        }

        // Method to fetch rooms by size
        public async void FetchRoomsBySize(string roomSize, DateTime checkIn)
        {
            const string query = @"
                SELECT
                    r.size AS room_size,
                    r.id AS room_id,
                    r.price,
                    r.description AS room_description,
                    r.balcony,
                    h.name AS hotel_name,
                    h.address AS hotel_address,
                    c.name AS city_name,
                    cn.name AS country_name
                FROM
                    Room r
                LEFT JOIN
                    Hotel h ON r.hotel_id = h.id
                LEFT JOIN
                    City c ON h.city_id = c.id
                LEFT JOIN
                    Country cn ON h.country_id = cn.id
                LEFT JOIN
                    Booking_x_rooms bxr ON r.id = bxr.room_id
                LEFT JOIN
                    Booking b ON bxr.booking_id = b.id
                WHERE
                    r.size = $1
                    AND (
                        b.id IS NULL OR 
                        NOT (
                            b.start_date <= $2 AND $2 < b.start_date + INTERVAL '1 DAY'
                        )
                    )
                ORDER BY
                    h.name, r.id;";

            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(roomSize); // $1
                cmd.Parameters.AddWithValue(checkIn);  // $2
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine("Rooms by Size:");
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"Room ID: {reader.GetInt32(1)}, Size: {reader.GetString(0)}, Hotel: {reader.GetString(5)}, City: {reader.GetString(7)}");
                    }
                }
            }
        }

        // Method to fetch hotels by amenities (e.g., Pool, Entertainment, etc.)
        public async void FetchHotelsByAmenity(string amenity, int hotelId)
        {
            string query = $@"
                SELECT
                    h.name AS hotel_name,
                    h.address AS hotel_address,
                    h.{amenity}
                FROM
                    Hotel h
                WHERE
                    h.id = $1;";

            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(hotelId); // $1
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"Hotel: {reader.GetString(0)}, Address: {reader.GetString(1)}, {amenity}: {reader.GetBoolean(2)}");
                    }
                }
            }
        }

        // Method to fetch hotels close to city center or beach
        public async void FetchHotelsByProximity(bool closeToCityCenter, int? hotelId = null)
        {
            string query;

            if (hotelId.HasValue)
            {
                query = closeToCityCenter
                    ? "SELECT h.name, h.address, h.city_proximity FROM Hotel h WHERE h.id = $1;"
                    : "SELECT h.name, h.address, h.beach_proximity FROM Hotel h WHERE h.id = $1;";
            }
            else
            {
                query = closeToCityCenter
                    ? "SELECT h.name, c.name AS city_name, h.city_proximity FROM Hotel h JOIN City c ON h.city_id = c.id ORDER BY h.city_proximity;"
                    : "SELECT h.name, c.name AS city_name, h.beach_proximity FROM Hotel h JOIN City c ON h.city_id = c.id ORDER BY h.beach_proximity;";
            }

            await using (var cmd = _db.CreateCommand(query))
            {
                if (hotelId.HasValue)
                    cmd.Parameters.AddWithValue(hotelId.Value);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine(closeToCityCenter
                            ? $"Hotel: {reader.GetString(0)}, Distance to City: {reader.GetDecimal(2)} km"
                            : $"Hotel: {reader.GetString(0)}, Distance to Beach: {reader.GetDecimal(2)} km");
                    }
                }
            }
        }

        // Method to fetch rooms ordered by price
        public async void FetchRoomsByPrice(int hotelId, string roomSize)
        {
            const string query = @"
                SELECT
                    r.id AS room_id,
                    r.size AS room_size,
                    r.price AS room_price,
                    h.name AS hotel_name,
                    h.address AS hotel_address
                FROM
                    Room r
                JOIN
                    Hotel h ON r.hotel_id = h.id
                WHERE
                    h.id = $1 AND r.size = $2
                ORDER BY
                    r.price;";

            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue(hotelId); // $1
                cmd.Parameters.AddWithValue(roomSize); // $2
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    Console.WriteLine("Rooms by Price:");
                    while (await reader.ReadAsync())
                    {
                        Console.WriteLine($"Room ID: {reader.GetInt32(0)}, Size: {reader.GetString(1)}, Price: {reader.GetDecimal(2)}, Hotel: {reader.GetString(3)}");
                    }
                }
            }
        }
    



    
}