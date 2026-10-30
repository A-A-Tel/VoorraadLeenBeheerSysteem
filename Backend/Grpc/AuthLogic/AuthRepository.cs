using Backend.Database;
using Backend.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Backend.Grpc.AuthLogic;

public class AuthRepository(AppDbContext database)
{
    public async Task<User?> GetUserAsync(byte[] cardBytes)
    {
        return await database.Users
            .Where(u => u.CardBytes == cardBytes)
            .Include(u => u.Role)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserAsync(string email)
    {
        return await database.Users
            .Where(u => u.Email == email)
            .Include(u => u.Role)
            .FirstOrDefaultAsync();
    }

    public async Task RehashPasswordAsync(User user, string password) { }
}