using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Data.Contexts;

public class AlphaDbContext(DbContextOptions<AlphaDbContext> options) : IdentityDbContext(options)
{
    
}
