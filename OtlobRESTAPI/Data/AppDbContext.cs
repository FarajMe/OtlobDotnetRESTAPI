using Microsoft.EntityFrameworkCore;
using OtlobRESTAPI.Data.Models;

namespace OtlobRESTAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        public DbSet<Lookup> lookup { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<Restaurant> restaurant { get; set; }
        public DbSet<Vehicle> vehicle { get; set; }
        public DbSet<Driver> driver { get; set; }
        public DbSet<Orders> orders { get; set; }
        public DbSet<Delivery> delivery { get; set; }
        public DbSet<MenuItem> menuitem { get; set; }
        public DbSet<OrderItem> orderitem { get; set; }
        public DbSet<RestaurantRequest> restaurantrequest { get; set; }
    }
}
