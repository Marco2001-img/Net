using back.Models;
using Microsoft.EntityFrameworkCore;

namespace back.DTOS
{
    public class appDBcontext : DbContext
    {
        public appDBcontext(DbContextOptions<appDBcontext> options) : base(options) { }

        public DbSet<Persona> Persons { get; set; }
    }
}
