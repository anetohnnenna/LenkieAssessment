using LibraryApi.Entities;
using LibraryApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<RegistrationModel> RegistrationTable { get; set; }
       
        public DbSet<BookOrder> BookOrdersTable { get; set; }
        public DbSet<LibraryBook> LibraryBooksTable { get; set; }
    }
}
