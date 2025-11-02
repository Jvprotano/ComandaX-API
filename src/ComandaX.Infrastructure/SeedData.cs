using ComandaX.Domain.Entities;

namespace ComandaX.Infrastructure;

public class SeedData
{
    public static async Task SeedAdminsAsync(AppDbContext context)
    {
        var admins = new[]
        {
            "jvprotano@gmail.com",
            "vhprotano@gmail.com",
            "protanosoftware@gmail.com"
        };

        foreach (var email in admins)
        {
            if (!context.Users.Any(u => u.Email == email))
            {
                context.Users.Add(new User(email, "Admin"));
            }
        }

        await context.SaveChangesAsync();
    }

    public static async Task GenerateDevTestData()
    {

    }
}
