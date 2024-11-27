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
                    Console.WriteLine($"id: {reader.GetInt32(0)} \t name: {reader.GetString(1)} \t slogan: {reader.GetString(2)}");
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

    public void RegisterCustomer(String firstName, String lastName, String email, String phone)
    {
        
    }

    public void ModifyCustomer(String email)
    {
        
    }

    public bool DeleteCustomer(String email)
    {
        return true;
    }
}
