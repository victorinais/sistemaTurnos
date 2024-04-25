using Microsoft.EntityFrameworkCore;
using sistemaTurnos.Models;

namespace sistemaTurnos.Data;

public class BaseContext : DbContext
{
    public BaseContext(DbContextOptions<BaseContext> options) : base(options){

    }

    public DbSet<Usuario> Usuarios { get; set; }
}